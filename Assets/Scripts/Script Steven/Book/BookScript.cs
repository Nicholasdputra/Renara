using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookScript : MonoBehaviour
{
    GameObject player;
    [Header("Tool Sprites")]
    //U dont have to use an SO for this, it could be changed to a sprite array

    public ToolDataSO toolData;
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

    private void Start()
    {
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        prevButton.gameObject.SetActive(false);
        openPageIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        //ini di run sekali aj udh ke save ke SO
        // PlantSO[] plants = Resources.LoadAll<PlantSO>("PlantData");
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.B)) && leftBookPanel.gameObject.activeSelf)
        {
            //close book
            // gameObject.SetActive(false);
            openPageIndex = 0;
            player.GetComponent<CharacterMovement>().canMove = true;
            gameObject.GetComponent<Image>().enabled = false;
            leftBookPanel.gameObject.SetActive(false);
            rightBookPanel.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            prevButton.gameObject.SetActive(false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.B) && player.GetComponent<CharacterMovement>().canMove && !leftBookPanel.gameObject.activeSelf)
        {
            //open book
            OpenBook();
        }
    }

    public void OpenBook()
    {
        player.GetComponent<CharacterMovement>().canMove = false;
        gameObject.GetComponent<Image>().enabled = true;
        leftBookPanel.gameObject.SetActive(true);
        rightBookPanel.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        //since first page, there is no prev page
        prevButton.gameObject.SetActive(false);
        UpdatePage();
    }

    int DetermineWhichToolSpriteToUse(string extractionStep)
    {
        switch (extractionStep)
        {
            case "Hoe":
                return 0;
            case "GardeningScissors":
                return 1;
            case "SprayPotion":
                return 2;
            case "Sickle":
                return 3;
            case "Shovel":
                return 4;
            case "Axe":
                return 5;
            default:
                Debug.LogError("Unknown tool: " + extractionStep);
                return -1; // Unknown tool
        }
    }

    [ContextMenu("NextPage")]
    public void NextPage()
    {
        //check if there are more plants to show
        if (openPageIndex < plantDataSO.plant.Length / 2)
        {
            openPageIndex++;
            // Debug.Log("Open Page Index: " + openPageIndex);
            // Debug.Log("max " + plantDataSO.plant.Length / 2);
            if (openPageIndex == (plantDataSO.plant.Length / 2)-1)
            {
                //last page, remove next button
                // Debug.Log("removing next button");
                nextButton.gameObject.SetActive(false);
            }
            else
            {
                nextButton.gameObject.SetActive(true);
            }
            //pressing next always means there is a prev page
            prevButton.gameObject.SetActive(true);
            UpdatePage();
        }
    }

    [ContextMenu("PrevPage")]
    public void PrevPage()
    {
        //check if there are more plants to show
        if (openPageIndex != 0)
        {
            openPageIndex--;
            if (openPageIndex == 0)
            {
                //first page, remove prev button
                prevButton.gameObject.SetActive(false);
            }
            else
            {
                prevButton.gameObject.SetActive(true);
            }
            //pressing prev always means there is a next page
            nextButton.gameObject.SetActive(true);
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        // Debug.Log("Updating page: " + openPageIndex);
        leftBookPanel.gameObject.SetActive(true);
        rightBookPanel.gameObject.SetActive(true);
        //Clear extraction steps
        ClearLeftPageSteps();
        ClearRightPageSteps();

        UpdateLeftPage();
        UpdateRightPage();
    }

    void ClearLeftPageSteps()
    {
        extractionSteps = leftBookPanel.GetChild(6);
        for (int i = 0; i < extractionSteps.childCount; i++)
        {
            Destroy(extractionSteps.GetChild(i).gameObject);
        }
    }

    void ClearRightPageSteps()
    {
        extractionSteps = rightBookPanel.GetChild(6);
        for (int i = 0; i < extractionSteps.childCount; i++)
        {
            Destroy(extractionSteps.GetChild(i).gameObject);
        }
    }

    void UpdateLeftPage()
    {
        //update left page
        plantImage = leftBookPanel.GetChild(2).GetComponent<Image>();
        plantNameText = leftBookPanel.GetChild(3).GetComponent<TMP_Text>();
        plantDescText = leftBookPanel.GetChild(4).GetComponent<TMP_Text>();
        amountExtractedText = leftBookPanel.GetChild(5).GetComponent<TMP_Text>();
        extractionSteps = leftBookPanel.GetChild(6);

        PlantSO plant;
        bool plantUnlocked = plantDataSO.plant[openPageIndex * 2].isUnlocked;
        if (plantUnlocked)
        {
            //if unlocked show normal data
            plant = plantDataSO.plant[openPageIndex * 2];
        }
        else
        {
            //else show unknown plant data (question mark)
            plant = plantDataSO.unknownPlantData;
        }
        plantImage.sprite = plant.plantImage;
        plantNameText.text = plant.plantName;
        // Debug.Log("Plant Name: " + plant.plantName);
        plantDescText.text = plant.plantDescription;
        amountExtractedText.text = PlayerPrefs.GetInt(plant.plantName).ToString();
        //spawn extraction steps
        for (int i = 0; i < plant.extractionSteps.Length; i++)
        {
            GameObject tool = Instantiate(toolsPrefab, extractionSteps);
            // Debug.Log("Showing" + plant.extractionSteps[i]);
            //set sprite inside of the circle
            if (plantUnlocked)
            {
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolData.tools[DetermineWhichToolSpriteToUse(plant.extractionSteps[i])].toolSprite;
            }
            else
            {
                //unknown tool is the last index of the toolSprites array
                //Just spawn 1 unknown step and finish
                // Debug.Log("Unknown tool, showing unknown tool sprite");
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolData.tools[^1].toolSprite;
                break;
            }
            //spawn plus if not the last index
            // if (i != plant.extractionSteps.Length - 1)
            // {
            //     Instantiate(plusPrefab, extractionSteps);
            // }
        }
    }

    void UpdateRightPage()
    {
        //update right page (basically same thing but add 1 to the index)
        //need to check whether right plant exists or not
        if (openPageIndex * 2 + 1 >= plantDataSO.plant.Length)
        {
            //if not exist, hide the right page
            rightBookPanel.gameObject.SetActive(false);
            return;
        }
        PlantSO plant;
        bool plantUnlocked = plantDataSO.plant[openPageIndex * 2 + 1].isUnlocked;
        plantImage = rightBookPanel.GetChild(2).GetComponent<Image>();
        plantNameText = rightBookPanel.GetChild(3).GetComponent<TMP_Text>();
        plantDescText = rightBookPanel.GetChild(4).GetComponent<TMP_Text>();
        amountExtractedText = rightBookPanel.GetChild(5).GetComponent<TMP_Text>();
        extractionSteps = rightBookPanel.GetChild(6);

        if (plantUnlocked)
        {
            //if unlocked show normal data
            plant = plantDataSO.plant[openPageIndex * 2 + 1];
        }
        else
        {
            //else show unknown plant data (question mark)
            plant = plantDataSO.unknownPlantData;
        }
        plantImage.sprite = plant.plantImage;
        plantNameText.text = plant.plantName;
        // Debug.Log("Plant Name: " + plant.plantName);
        plantDescText.text = plant.plantDescription;
        if (plantUnlocked)
        {
            amountExtractedText.text = PlayerPrefs.GetInt(plant.plantName).ToString();
        }
        else
        {
            amountExtractedText.text = "0";
        }
        //spawn extraction steps
        for (int i = 0; i < plant.extractionSteps.Length; i++)
        {
            GameObject tool = Instantiate(toolsPrefab, extractionSteps);
            //set sprite inside of the circle
            if (plantUnlocked)
            {
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolData.tools[DetermineWhichToolSpriteToUse(plant.extractionSteps[i])].toolSprite;
            }
            else
            {
                //unknown tool is the last index of the toolSprites array
                //Just spawn 1 unknown step and finish
                //^1 is index from end kyk python negative index
                tool.transform.GetChild(0).GetComponent<Image>().sprite = toolData.tools[^1].toolSprite;
                break;
            }
            //spawn plus if not the last index
            if (i != plant.extractionSteps.Length - 1)
            {
                Instantiate(plusPrefab, extractionSteps);
            }
        }
    }
}
