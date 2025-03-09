using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingManager : MonoBehaviour
{
    [Header("For Displaying What Items Can Be Crafted")]
    public ItemDataSO ItemDataSO;
    // public MaterialDataSO MaterialDataSO;
    public GameObject itemButtonPrefab;
    public GameObject itemButtonParent;
    public GameObject craftablePopUp;
    public GameObject materialsNeededParent;
    public GameObject materialsNeededPrefab;

    [Header("For Crafting Minigame")]
    public bool isInMinigame;
    public GameObject craftingMinigameObjects;
    public bool partOneDone;
    public bool partTwoDone;
    public bool partThreeDone;
    public Slider partOneSlider;
    public Slider partTwoSlider;
    public Slider partThreeSlider;
    public Image coloredZone;

    public void OnEnable()
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
        // if(isInMinigame){
        //     if(!partOneDone){

        //     }
        // }
    }

    void CraftingListButton(ItemSO item)
    {
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

    // public IEnumerator CraftingMinigameCoroutine(Slider slider, Image coloredZone)
    // {
    //     float direction = 1f;
    //     while (true)
    //     {
    //         if (slider.value >= slider.maxValue)
    //         {
    //             direction = -1f;
    //         }
    //         else if (slider.value <= slider.minValue)
    //         {
    //             direction = 1f;
    //         }
    //         slider.value += direction * Time.deltaTime;
    //         yield return null;
    //     }
    // }
    

    // void CraftingMinigame(){
    //     //Set the minigame items to active

    //     //Set the minigame items to inactive
    // }
}
