using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingManager : MonoBehaviour
{
    [Header("For Displaying What Items Can Be Crafted")]
    public GameObject craftingListObjects;
    public ItemDataSO ItemDataSO;
    // public MaterialDataSO MaterialDataSO;
    public GameObject itemButtonPrefab;
    public GameObject itemButtonParent;
    public GameObject craftablePopUp;
    public GameObject materialsNeededParent;
    public GameObject materialsNeededPrefab;
    public GameObject craftButton;

    [Header("For Crafting Minigame")]
    public Coroutine sliderCoroutine;
    public bool isInMinigame;
    public GameObject craftingMinigameObjects;
    public int minigameProg;
    public Slider[] sliders;
    public Image[] indicators;
    public float maxVal;
    public float minVal;
    public Image coloredZone;
    public ItemSO activeItem;

    public void SetUpCraftingList()
    {
        // string obtainedMaterialsLog = "Obtained Materials:\n";
        // foreach (CraftingMaterial craftingMaterial in SaveSystem.currentSave.currentPlayerData.obtainedMaterials)
        // {
        //     obtainedMaterialsLog += $"{craftingMaterial.materialSO.materialName} x{craftingMaterial.amount}\n";
        // }
        // Debug.Log(obtainedMaterialsLog);

        //Determine which buttons to display based on the items in the player's inventory
        //you do this by comparing it to the recipes for every item
        foreach (ItemSO item in ItemDataSO.items)
        {
            bool craftable = true;
            // Debug.Log("Current Item Needs: " +  item);
            foreach (CraftingMaterial requiredMaterial in item.materialsNeeded)
            {
                // Debug.Log(requiredMaterial.amount + " "+ requiredMaterial.materialSO.materialName);

                //Check if the player has a sufficient amount of each material
                CraftingMaterial playerMaterial = SaveSystem.currentSave.currentPlayerData.obtainedMaterials.Find(m => m.materialSO == requiredMaterial.materialSO);
                if (playerMaterial == null || playerMaterial.amount < requiredMaterial.amount)
                {
                    // Debug.Log("Player does not have enough " + requiredMaterial.materialSO.materialName);
                    craftable = false;
                    break;
                } 
                // else{
                //     Debug.Log("Player has enough " + requiredMaterial.materialSO.materialName + ". They have: " + playerMaterial.amount);
                // }
            }
            //If the player has all the materials, display the button
            Debug.Log(item + " is craftable: " + craftable);
            if (craftable)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemButtonParent.transform);
                //Set the button's text to the item's name
                // Debug.Log("Item Name: " + item.itemName);
                // Debug.Log("Item Button: " + itemButton);

                //This is necessary because otherwise it'll change the button's sprite instead of the icon's sprite to the item
                foreach (Transform child in itemButton.transform)
                {
                    Debug.Log("Child Name: " + child.name);
                    TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        //Set the button's text to the item's name
                        Debug.Log("Text Component Found: " + textComponent.text);
                        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
                    }

                    Image imageComponent = child.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        //Set the button's icon to the item's icon
                        Debug.Log("Image Component Found: " + imageComponent.sprite);
                        imageComponent.sprite = item.itemSprite;
                        // itemButton.GetComponentInChildren<Image>().sprite = item.itemSprite;
                    }
                }
                //Add a listener to the button so that when it's clicked, the craftablePopUp will open and work as intended
                itemButton.GetComponent<Button>().onClick.AddListener(() => CraftingListButton(item));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isInMinigame){
            if(minigameProg == 0){ //A
                if(sliderCoroutine == null){
                    StartCraftingMinigamePartX();
                }
                if(Input.GetKey(KeyCode.A)){
                    Debug.Log("A");                    
                    DetermineCraftingMinigameOutcome();
                }
            } else if(minigameProg == 1){ //S
                if(sliderCoroutine == null){
                    StartCraftingMinigamePartX();
                }
                if(Input.GetKey(KeyCode.S)){
                    Debug.Log("S");
                    DetermineCraftingMinigameOutcome();
                }
            } else if(minigameProg == 2){ //D
                if(sliderCoroutine == null){
                    StartCraftingMinigamePartX();
                }
                if(Input.GetKey(KeyCode.D)){
                    Debug.Log("D");
                    DetermineCraftingMinigameOutcome();
                }
            } else if(minigameProg == 3){
                isInMinigame = false;
                CraftItem();
            }
        }
    }

    void StartCraftingMinigamePartX(){
        CreateColoredZone(sliders[minigameProg]);
        sliderCoroutine = StartCoroutine(SliderCoroutine(sliders[minigameProg]));
    }

    void DetermineCraftingMinigameOutcome(){
        if(sliders[minigameProg].value >= minVal/100 && sliders[minigameProg].value <= maxVal/100){
            Debug.Log("Success");
            indicators[minigameProg].color = Color.green;
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null;
            if(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject != null){
                Destroy(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject);
            }
            minigameProg++;
            
        }
        else{
            Debug.Log("Fail");
            Debug.Log("Min: " + minVal + " Max: " + maxVal + " Value: " + sliders[minigameProg].value);
            Debug.Log("Minigame Progress: " + minigameProg);
            Debug.Log("Slider Value: " + sliders[minigameProg].value);
            for(int i = minigameProg; i >= 0; i--){
                indicators[i].color = Color.red;
                if(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject != null){
                    Destroy(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject);
                }
                sliders[i].value = 0;
            } 
            minigameProg = 0;
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null;
        }
    }

    void CraftingListButton(ItemSO item)
    {
        activeItem = item;
        //Open the craftablePopUp
        craftablePopUp.SetActive(true);
        //Set the image to the item's icon
        craftablePopUp.GetComponentInChildren<Image>().sprite = item.itemSprite;
        //Set the materials needed to the item's materials needed
        foreach (CraftingMaterial craftingMaterial in item.materialsNeeded)
        {
            GameObject materialNeeded = Instantiate(materialsNeededPrefab, materialsNeededParent.transform);
            materialNeeded.GetComponentInChildren<TextMeshProUGUI>().text = craftingMaterial.amount.ToString();
            Transform materialTypeIconTransform = materialNeeded.transform.Find("MaterialTypeIcon");
            if (materialTypeIconTransform != null)
            {
                Image materialTypeIcon = materialTypeIconTransform.GetComponent<Image>();
                materialTypeIcon.sprite = craftingMaterial.materialSO.materialSprite;
            }
        }
    }

    void CraftItem(){
        //Remove the materials needed from the player's inventory, make sure it's the right amout
        foreach (CraftingMaterial requiredMaterial in activeItem.materialsNeeded)
        {
            // Debug.Log(requiredMaterial.amount + " "+ requiredMaterial.materialSO.materialName);
            //Check if the player has a sufficient amount of each material
            CraftingMaterial playerMaterial = SaveSystem.currentSave.currentPlayerData.obtainedMaterials.Find(m => m.materialSO == requiredMaterial.materialSO);
            if (playerMaterial.amount == requiredMaterial.amount)
            {
                SaveSystem.currentSave.currentPlayerData.obtainedMaterials.Remove(playerMaterial);
            }
            else if (playerMaterial.amount > requiredMaterial.amount)
            {
                playerMaterial.amount -= requiredMaterial.amount;
            } 
            else
            {
                //This should never happen
                Debug.LogError("Player does not have enough " + requiredMaterial.materialSO.materialName);
            }
        }

        //Add the crafted item to the player's inventory
        SaveSystem.currentSave.currentPlayerData.itemDataSO.items.Add(activeItem);
        
        //Close the craftablePopUp
        craftingMinigameObjects.SetActive(false);
        craftablePopUp.SetActive(false);
        SetUpCraftingList();
    }

    public void Start()
    {
        isInMinigame = false;
        minigameProg = 0;
        sliderCoroutine = null;
        SetUpCraftingList();
        craftingMinigameObjects.SetActive(false);
        craftablePopUp.SetActive(false);
    }

    public IEnumerator SliderCoroutine(Slider slider)
    {
        bool goingUp = true;
        while (true)
        {
            if (slider.value == slider.maxValue)
            {
                goingUp = false;
            }
            else if (slider.value == slider.minValue)
            {
                goingUp = true;
            }

            if(goingUp){
                slider.value += Time.deltaTime;
            } else{
                slider.value -= Time.deltaTime;
            }
            yield return null;
        }
    }

    void CreateColoredZone(Slider partXSlider){
        float randomNum = Random.Range(11, 90);
        Debug.Log(randomNum);
        minVal = randomNum - 10;
        maxVal = randomNum + 10;

        // Instantiate the colored zone and set its parent
        Image instantiatedColoredZone = Instantiate(coloredZone, partXSlider.transform);
        
        // Set the size and position of the colored zone
        RectTransform coloredZoneRect = instantiatedColoredZone.GetComponent<RectTransform>();
        float sliderWidth = partXSlider.transform.Find("Background").GetComponent<RectTransform>().rect.width;
        
        // Debug.Log("Slider Width:" + sliderWidth);
        float sliderHeight = partXSlider.GetComponent<RectTransform>().rect.height;
        
        // Debug.Log("Slider Height:" + sliderHeight);
        float coloredZoneWidth = sliderWidth;
        float coloredZoneHeight = (maxVal - minVal) * sliderHeight / 100; 
        coloredZoneRect.sizeDelta = new Vector2(coloredZoneWidth, coloredZoneHeight);
        Vector3 coloredZonePos = Vector3.zero; 
        coloredZonePos.y -= sliderHeight / 2;
        coloredZonePos.y += randomNum / 100 * sliderHeight;
        coloredZoneRect.anchoredPosition = coloredZonePos;
    }
}