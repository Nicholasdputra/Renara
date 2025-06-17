using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    [Header("For Displaying What Items Can Be Crafted")]
    public GameObject craftingListObjects; //The crafting list that shows what items can be crafted
    public GameObject craftableItemButtonPrefab;//prefab for the craftable item button
    public GameObject uncraftableItemButtonPrefab; //prefab for the uncraftable item button
    public GameObject itemButtonParent; //The parent of the buttons that show what items can be crafted
    public GameObject craftablePopUp; //The pop up that shows the item you can craft
    private List<ItemSO> uncraftableItems = new List<ItemSO>(); //List of items that cannot be crafted right now
    public GameObject craftablePopUpImageGameObj; //The image of the item that can be crafted in the craftablePopUp
    public GameObject materialsNeededParent; //The place where the materials needed to craft an item are displayed
    public GameObject materialsNeededPrefab; //Prefab for the materials needed to craft an item
    public GameObject craftButton; //The button you click that crafts the item and adds it to the inventory
    public GameObject backButton; //To exit the crafting table 

    [Header("For Crafting Minigame")]
    private Coroutine sliderCoroutine; //the coroutine that moves the slider
    private bool isInMinigame; //bool to check if the player is in the crafting minigame
    public GameObject craftingMinigameObjects; //assortment of objects that are used in the crafting minigame
    private int minigameProg; //the current progress of the crafting minigame (1st part, 2nd part, etc.)
    public Slider[] sliders; //the sliders that are used in the crafting minigame
    public Image[] indicators; //the indicators at the bottom of the sliders
    private float maxVal; //the maximum value of the slider
    private float minVal; //the minimum value of the slider
    public Image coloredZone; //the colored zone that shows the player where to stop the slider
    private ItemSO activeItem; //the item that is currently being crafted

    public void EnterCraftingTable(){
        gameObject.SetActive(true);
    }

    public void ExitCraftingTable()
    {
        gameObject.SetActive(false);
        if (craftingListObjects.activeSelf)
        {
            craftingListObjects.SetActive(false);
        }
        if (craftingMinigameObjects.activeSelf)
        {
            craftingMinigameObjects.SetActive(false);
        }
        if (craftablePopUp.activeSelf)
        {
            craftablePopUp.SetActive(false);
        }
        if (backButton.activeSelf)
        {
            backButton.SetActive(false);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterMovement>().canMove = true;
        SaveSystem.currentSave.Save();
    }

    public void OnEnable()
    {
        backButton.SetActive(true);
        isInMinigame = false;
        minigameProg = 0;
        sliderCoroutine = null;
        SetUpCraftingList();
        craftingListObjects.SetActive(true);
        craftingMinigameObjects.SetActive(false);
        craftablePopUp.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("Escape");
            ExitCraftingTable();
            return;
        }

        if(isInMinigame){
            if(minigameProg == 0){ //A
                if(sliderCoroutine == null){
                    StartCraftingMinigamePartX();
                }
                if(Input.GetKeyDown(KeyCode.A)){                  
                    DetermineCraftingMinigameOutcome();
                }
            } else if(minigameProg == 1){ //S
                if(sliderCoroutine == null){
                    StartCraftingMinigamePartX();
                }
                if(Input.GetKeyDown(KeyCode.S)){
                    DetermineCraftingMinigameOutcome();
                }
            } else if(minigameProg == 2){ //D
                if(sliderCoroutine == null){
                    StartCraftingMinigamePartX();
                }
                if(Input.GetKeyDown(KeyCode.D)){
                    DetermineCraftingMinigameOutcome();
                }
            } else if(minigameProg == 3){
                isInMinigame = false;
                CraftItem();
            }
        }
    }

    public void SetUpCraftingList()
    {
        //Clear the crafting list
        if(itemButtonParent.transform.childCount > 0)
        {
            foreach (Transform child in itemButtonParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //Clear the uncraftable items list
        uncraftableItems.Clear();

        //Determine which buttons to display based on the items in the player's inventory
        //you do this by comparing it to the recipes for every item
        foreach (ItemSO item in SaveSystem.currentSave.currentPlayerData.listOfRecipes.items)
        {
            bool craftable = true;
            foreach (CraftingMaterial requiredMaterial in item.materialsNeeded)
            {
                //Check if the player has a sufficient amount of each material
                CraftingMaterial playerMaterial = SaveSystem.currentSave.currentPlayerData.obtainedMaterials.Find(m => m.materialSO == requiredMaterial.materialSO);
                if (playerMaterial == null || playerMaterial.amount < requiredMaterial.amount)
                {
                    craftable = false;
                    break;
                } 
            }

            //If the player has all the materials, display the button
            if (craftable)
            {
                GameObject itemButton = Instantiate(craftableItemButtonPrefab, itemButtonParent.transform);
                //Set the button's text to the item's name
                //This is necessary because otherwise it'll change the button's sprite instead of the icon's sprite to the item
                foreach (Transform child in itemButton.transform)
                {
                    TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        //Set the button's text to the item's name
                        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
                    }

                    Image imageComponent = child.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        //Set the button's icon to the item's icon
                        imageComponent.sprite = item.itemSprite;
                    }
                }
                //Add a listener to the button so that when it's clicked, the craftablePopUp will open and work as intended
                itemButton.GetComponent<Button>().onClick.AddListener(() => CraftingListButton(item));
            } 
            else
            {
                //add to uncraftable items list
                uncraftableItems.Add(item);
            }
        }
        //Display the uncraftable items, this is here so that all the uncraftables are displayed at the end
        foreach (ItemSO item in uncraftableItems)
        {
            GameObject itemButton = Instantiate(uncraftableItemButtonPrefab, itemButtonParent.transform);
            //Set the button's text to the item's name
            //This is necessary because otherwise it'll change the button's sprite instead of the icon's sprite to the item
            foreach (Transform child in itemButton.transform)
            {
                TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    //Set the button's text to the item's name
                    itemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
                }
                Image imageComponent = child.GetComponent<Image>();
                if (imageComponent != null)
                {
                    //Set the button's icon to the item's icon
                    imageComponent.sprite = item.itemSprite;
                }
                itemButton.GetComponent<Button>().onClick.AddListener(() => UncraftableItemCraftingListButton(item));
            }
        }
    }

    void CraftingListButton(ItemSO item)
    {
        //Reset the materials needed children (in case it wasnt properly reset)
        if(materialsNeededParent.transform.childCount > 0)
        {
            foreach (Transform child in materialsNeededParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        //Set the active item to the item that was clicked
        activeItem = item;

        //Open the craftablePopUp
        craftablePopUp.SetActive(true);

        //Set the image to the item's icon
        craftablePopUpImageGameObj.GetComponentInChildren<Image>().sprite = item.itemSprite;

        //Set the materials needed to the item's materials needed
        foreach (CraftingMaterial craftingMaterial in item.materialsNeeded)
        {
            //We make the image of the material pop up on the right
            GameObject materialNeeded = Instantiate(materialsNeededPrefab, materialsNeededParent.transform);
            materialNeeded.GetComponentInChildren<TextMeshProUGUI>().text = craftingMaterial.amount.ToString();
            Transform materialTypeIconTransform = materialNeeded.transform.Find("MaterialTypeIcon");
            if (materialTypeIconTransform != null)
            {
                Image materialTypeIcon = materialTypeIconTransform.GetComponent<Image>();
                materialTypeIcon.sprite = craftingMaterial.materialSO.materialSprite;
            }
        }

        //Set the craft button to be active
        craftButton.SetActive(true);
    }

    void UncraftableItemCraftingListButton(ItemSO item)
    {
        //Reset the materials needed children (in case it wasnt properly reset)
        if(materialsNeededParent.transform.childCount > 0)
        {
            foreach (Transform child in materialsNeededParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        //Set the active item to the item that was clicked
        activeItem = item;

        //Open the craftablePopUp
        craftablePopUp.SetActive(true);

        //Set the image to the item's icon
        craftablePopUp.GetComponentInChildren<Image>().sprite = item.itemSprite;

        //Set the materials needed to the item's materials needed
        foreach (CraftingMaterial craftingMaterial in item.materialsNeeded)
        {
            //We make the image of the material pop up on the right
            GameObject materialNeeded = Instantiate(materialsNeededPrefab, materialsNeededParent.transform);
            materialNeeded.GetComponentInChildren<TextMeshProUGUI>().text = craftingMaterial.amount.ToString();
            Transform materialTypeIconTransform = materialNeeded.transform.Find("MaterialTypeIcon");
            if (materialTypeIconTransform != null)
            {
                Image materialTypeIcon = materialTypeIconTransform.GetComponent<Image>();
                materialTypeIcon.sprite = craftingMaterial.materialSO.materialSprite;
            }
        }

        //Set the craft button to be inactive
        craftButton.SetActive(false);
    }

    public void StartCraftingMinigame()
    {
        isInMinigame = true;
        craftingListObjects.SetActive(false);
        craftingMinigameObjects.SetActive(true);
        craftablePopUp.SetActive(false);
    }

    void StartCraftingMinigamePartX(){
        //Clear indicator colors
        for (int i = minigameProg; i < indicators.Length; i++)
        {
            Transform coloredZone = sliders[i].transform.Find("ColoredZone(Clone)");
            if (coloredZone != null) {
                Destroy(coloredZone.gameObject);
            }   
            if (indicators[i].color != Color.white){
                indicators[i].color = Color.white;
            }
        }
        CreateColoredZone(sliders[minigameProg]);
        sliderCoroutine = StartCoroutine(SliderCoroutine(sliders[minigameProg]));
    }

    void DetermineCraftingMinigameOutcome()
    {
        //Check if the slider is in the right position
        if(sliders[minigameProg].value >= minVal/100 && sliders[minigameProg].value <= maxVal/100){
            //Set the indicator to green
            indicators[minigameProg].color = Color.green;

            //Make the slider stop moving
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null;

            //Destroy the colored zone
            if(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject != null){
                Destroy(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject);
            }

            //Increment the minigame progress
            minigameProg++;
        }
        else{
            //Reset the indicators and destroy the colored zone
            for(int i = minigameProg; i >= 0; i--){
                indicators[i].color = Color.white;
                if(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject != null){
                    Destroy(sliders[minigameProg].transform.Find("ColoredZone(Clone)").gameObject);
                }
                sliders[i].value = 0;
            } 

            //Reset the minigame progress
            minigameProg = 0;

            //Stop the slider coroutine
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null;
        }
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
        minVal = randomNum - 10;
        maxVal = randomNum + 10;

        // Instantiate the colored zone and set its parent
        Image instantiatedColoredZone = Instantiate(coloredZone, partXSlider.transform);
        instantiatedColoredZone.transform.SetSiblingIndex(1);
        
        // Get the slider's params
        RectTransform coloredZoneRect = instantiatedColoredZone.GetComponent<RectTransform>();
        float sliderWidth = partXSlider.transform.Find("Background").GetComponent<RectTransform>().rect.width;
        float sliderHeight = partXSlider.GetComponent<RectTransform>().rect.height;

        // Set the size of the colored zone
        float coloredZoneWidth = sliderWidth;
        float coloredZoneHeight = (maxVal - minVal) * sliderHeight / 100; 
        coloredZoneRect.sizeDelta = new Vector2(coloredZoneWidth, coloredZoneHeight);

        // Set the position of the colored zone
        Vector3 coloredZonePos = Vector3.zero; 
        coloredZonePos.y -= sliderHeight / 2;
        coloredZonePos.y += randomNum / 100 * sliderHeight;
        coloredZoneRect.anchoredPosition = coloredZonePos;
    }

    void CraftItem(){ 
        //Remove the materials needed from the player's inventory, make sure it's the right amout
        foreach (CraftingMaterial requiredMaterial in activeItem.materialsNeeded)
        {
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
        SaveSystem.currentSave.currentPlayerData.obtainedItemDataSO.items.Add(activeItem);
        
        //Close the craftablePopUp
        craftingMinigameObjects.SetActive(false);
        craftablePopUp.SetActive(false);

        //Reset everything
        isInMinigame = false;
        minigameProg = 0;
        SetUpCraftingList();
        sliderCoroutine = null;
        craftingListObjects.SetActive(true);
        craftingMinigameObjects.SetActive(false);
        craftablePopUp.SetActive(false);
    }

}