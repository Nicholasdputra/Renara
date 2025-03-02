using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BookScript : MonoBehaviour
{
    [Header("Tool Sprites")]
    //U dont have to use an SO for this, it could be changed to a sprite array

    public ToolSpritesSO toolSprites;
    [Header("Plant Data")]
    public PlantDataSO plantDataSO;
    [Header("UI References")]
    [SerializeField] Transform leftBookPanel;
    [SerializeField] Transform rightBookPanel;
    Image plantImage;
    TMP_Text plantNameText;
    TMP_Text plantDescText;
    TMP_Text amountExtractedText;
    Transform extractionSteps;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    
    [Header("Steps Prefabs")]
    //spawn prefab and plus in between it
    public GameObject toolsPrefab;
    public GameObject plusPrefab;
    public int openPageIndex;
    public bool isLeftMutated;
    public bool isRightMutated;

    void Start(){
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        prevButton.gameObject.SetActive(false);
        openPageIndex = 0;
        //ini di run sekali aj udh ke save ke SO
        // PlantSO[] plants = Resources.LoadAll<PlantSO>("PlantData");
        UpdatePage();
    }

    int DetermineWhichToolSpriteToUse(string extractionStep)
    {
        switch (extractionStep)
        {
            case "hoe":
                return 1;
            case "pruningShears":
                return 2;
            case "sprayBottle":
                return 3;
            case "sickle":
                return 4;
            case "spade":
                return 5;
            case "axe":
                return 6;
            default:
                Debug.LogError("Unknown tool: " + extractionStep);
                return -1; // Unknown tool
        }
    }

    [ContextMenu("NextPage")]
    public void NextPage(){
        //check if there are more plants to show
        if(openPageIndex < plantDataSO.plantData.Length/2){
            openPageIndex++;
            if(openPageIndex == plantDataSO.plantData.Length/2){
                //last page, remove next button
                nextButton.gameObject.SetActive(false);
            }else{
                nextButton.gameObject.SetActive(true);
            }
            //pressing next always means there is a prev page
            prevButton.gameObject.SetActive(true);
            UpdatePage();
        }
    }
    [ContextMenu("PrevPage")]
    public void PrevPage(){
        //check if there are more plants to show
        if(openPageIndex != 0){
            openPageIndex--;
            if(openPageIndex == 0){
                //first page, remove prev button
                prevButton.gameObject.SetActive(false);
            }else{
                prevButton.gameObject.SetActive(true);
            }
            //pressing prev always means there is a next page
            nextButton.gameObject.SetActive(true);
            UpdatePage();
        }
    }

    void UpdatePage(){
        leftBookPanel.gameObject.SetActive(true);
        rightBookPanel.gameObject.SetActive(true);
        //Clear extraction steps
        extractionSteps = leftBookPanel.GetChild(6);
        for(int i = 0; i < extractionSteps.childCount; i++){
            Destroy(extractionSteps.GetChild(i).gameObject);
        }
        extractionSteps = rightBookPanel.GetChild(6);
        for(int i = 0; i < extractionSteps.childCount; i++){
            Destroy(extractionSteps.GetChild(i).gameObject);
        }
        //update left page
        plantImage = leftBookPanel.GetChild(2).GetComponent<Image>();
        plantNameText = leftBookPanel.GetChild(3).GetComponent<TMP_Text>();
        plantDescText = leftBookPanel.GetChild(4).GetComponent<TMP_Text>();
        amountExtractedText = leftBookPanel.GetChild(5).GetComponent<TMP_Text>();
        extractionSteps = leftBookPanel.GetChild(6);

        PlantSO plant;
        bool plantUnlocked = plantDataSO.isUnlocked[openPageIndex*2];
        if(plantUnlocked){
            //if unlocked show normal data
            plant = plantDataSO.plantData[openPageIndex*2];
        }else{
            //else show unknown plant data (question mark)
            plant = plantDataSO.unknownPlantData;
        }
        plantImage.sprite = plant.plantImage;
        plantNameText.text = plant.plantName;
        plantDescText.text = plant.plantDescription;
        amountExtractedText.text = PlayerPrefs.GetInt(plant.plantName).ToString();
        //spawn extraction steps
        for(int i = 0; i < plant.extractionSteps.Length; i++){
            GameObject tool = Instantiate(toolsPrefab, extractionSteps);
            //set sprite inside of the circle
            if(plantUnlocked){
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolSprites.toolSprites[DetermineWhichToolSpriteToUse(plant.extractionSteps[i])];
            }else{
                //unknown tool is the last index of the toolSprites array
                //Just spawn 1 unknown step and finish
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolSprites.toolSprites[toolSprites.toolSprites.Length];
                break;
            }
            //spawn plus if not the last index
            if(i != plant.extractionSteps.Length - 1){
                Instantiate(plusPrefab, extractionSteps);
            }
        }

        //update right page (basically same thing but add 1 to the index)
        //need to check whether right plant exists or not
        if(openPageIndex*2+1 >= plantDataSO.plantData.Length){
            //if not exist, hide the right page
            rightBookPanel.gameObject.SetActive(false);
            return;
        }
        plantImage = rightBookPanel.GetChild(2).GetComponent<Image>();
        plantNameText = rightBookPanel.GetChild(3).GetComponent<TMP_Text>();
        plantDescText = rightBookPanel.GetChild(4).GetComponent<TMP_Text>();
        amountExtractedText = rightBookPanel.GetChild(5).GetComponent<TMP_Text>();
        extractionSteps = rightBookPanel.GetChild(6);

        plantUnlocked = plantDataSO.isUnlocked[openPageIndex*2+1];
        if(plantUnlocked){
            //if unlocked show normal data
            plant = plantDataSO.plantData[openPageIndex*2+1];
        }else{
            //else show unknown plant data (question mark)
            plant = plantDataSO.unknownPlantData;
        }
        plantImage.sprite = plant.plantImage;
        plantNameText.text = plant.plantName;
        plantDescText.text = plant.plantDescription;
        if(plantUnlocked){
            amountExtractedText.text = PlayerPrefs.GetInt(plant.plantName).ToString();
        }else{
            amountExtractedText.text = "0";
        }
        //spawn extraction steps
        for(int i = 0; i < plant.extractionSteps.Length; i++){
            GameObject tool = Instantiate(toolsPrefab, extractionSteps);
            //set sprite inside of the circle
            if(plantUnlocked){
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolSprites.toolSprites[DetermineWhichToolSpriteToUse(plant.extractionSteps[i])];
            }else{
                //unknown tool is the last index of the toolSprites array
                //Just spawn 1 unknown step and finish
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolSprites.toolSprites[toolSprites.toolSprites.Length-1];
                break;
            }
            //spawn plus if not the last index
            if(i != plant.extractionSteps.Length - 1){
                Instantiate(plusPrefab, extractionSteps);
            }
        }
    }

    public void ToggleMutatedImage(bool isRight){
        //Check which toggle is pressed, if left then change left sprite, else change right sprite
        if(!isRight){
            //left
            if(!plantDataSO.isUnlocked[openPageIndex*2]){
                //if not unlocked cant toggle
                return;
            }

            if(isLeftMutated){
                isLeftMutated = false;
                leftBookPanel.GetChild(2).GetComponent<Image>().sprite = plantDataSO.plantData[openPageIndex*2].plantImage;
            }else{
                isLeftMutated = true;
                leftBookPanel.GetChild(2).GetComponent<Image>().sprite = plantDataSO.plantData[openPageIndex*2].mutatedImage;
            }   
        }else{
            //right
            if(!plantDataSO.isUnlocked[openPageIndex*2+1]){
                //if not unlocked cant toggle
                return;
            }
            
            if(isRightMutated){
                isRightMutated = false;
                rightBookPanel.GetChild(2).GetComponent<Image>().sprite = plantDataSO.plantData[openPageIndex*2 + 1].plantImage;
            }else{
                isRightMutated = true;
                rightBookPanel.GetChild(2).GetComponent<Image>().sprite = plantDataSO.plantData[openPageIndex*2 + 1].mutatedImage;
            }
        }
    }
}
