using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabManager : MonoBehaviour
{
    int playerPlant;
    bool hasConfirmed;
    bool hasExtracted;
    bool hasReport;
    [Header ("Showing the background w the tube n plant")]
    [SerializeField] PlantDataSO plantData;
    [SerializeField] Image plantImage;
    [SerializeField] TMP_Text promptText;
    [Header ("Sequence References")]
    [SerializeField] DNAPanel dnaMatchingPanel;
    [SerializeField] DNAMatchingScript dnaGame;
    [SerializeField] TypingReportScript typingReportScript;
    [SerializeField] Milestone milestoneScript;
    [SerializeField] GameObject materialPanel;
    [SerializeField] GameObject materialPrefab;
    bool hasPlant;
    Image plantLabImage;
    CharacterMovement characterMovement;

    void Start()
    {
        characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        plantImage = transform.GetChild(1).GetComponent<Image>();
    }

    [ContextMenu("Start Lab")]
    public void StartLab()
    {
        playerPlant = SaveSystem.currentSave.currentPlayerData.currentPlant;
        promptText.gameObject.SetActive(true);
        if (playerPlant == -1)
        {
            hasPlant = false;
            plantImage.sprite = null;
            promptText.text = "You have no plant to extract DNA from. Press Space to exit.";
            plantImage.color = new Color(1, 1, 1, 0f);
        }
        else
        {
            hasPlant = true;
            plantImage.sprite = plantData.plant[playerPlant].plantImage;
            promptText.text = "Press Space to extract DNA";
            plantImage.color = new Color(1, 1, 1, 0.7176471f);
        }
        hasConfirmed = false;
        hasExtracted = false;
        hasReport = false;
        gameObject.SetActive(true);
        materialPanel.SetActive(false);
        promptText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!hasPlant)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                //close the lab
                gameObject.SetActive(false);
                characterMovement.canMove = true;
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !hasConfirmed && !hasExtracted)
        {
            hasConfirmed = true;
            dnaMatchingPanel.gameObject.SetActive(true);
            promptText.gameObject.SetActive(false);
            dnaGame.StartDNAExtraction();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && hasExtracted)
        {
            //close the lab
            gameObject.SetActive(false);
            for (int i = 1; i < materialPanel.transform.childCount; i++)
            {
                Destroy(materialPanel.transform.GetChild(i).gameObject);
            }
            materialPanel.SetActive(false);
            if (hasReport)
            {
                // milestoneScript.SubmitCurrentPlant();
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().canMove = true;
            }
            SaveSystem.currentSave.Save();
        }
    }

    [ContextMenu("Start Report")]
    public void CloseDNAWindow(){
        //Calls at the end of DNA Animation
        dnaMatchingPanel.gameObject.SetActive(false);
        // check if we have unlocked the plant or not
        if(plantData.plant[playerPlant].isUnlocked == false){
            //if not, unlock it
            //Unlocking is handled by the milestone script so this is not needed anymore
            // plantData.plant[playerPlant].isUnlocked = true;
            Debug.Log("Plant Unlocked");
            typingReportScript.gameObject.SetActive(true);
            typingReportScript.reportSO = plantData.plant[playerPlant].report;
            typingReportScript.StartReport();
        }
        else{
            //update plant extracted
            Debug.Log("Plant Already Unlocked");
            ShowExtractedMaterials(false);
        }
    }

    public void ShowExtractedMaterials(bool hasReport)
    {
        this.hasReport = hasReport;
        promptText.gameObject.SetActive(false);
        Debug.Log("Showing Extracted Materials");
        hasExtracted = true;
        materialPanel.SetActive(true);
        GameObject materialDrop = Instantiate(materialPrefab, materialPanel.transform);
        CraftingMaterialSO currentPlantDrop = plantData.plant[playerPlant].materialDrop;
        materialDrop.transform.GetChild(0).GetComponent<Image>().sprite = currentPlantDrop.materialSprite;
        materialDrop.transform.GetChild(1).GetComponent<TMP_Text>().text = currentPlantDrop.materialName;

        CraftingMaterial playerMaterial = SaveSystem.currentSave.currentPlayerData.obtainedMaterials.Find(m => m.materialSO == currentPlantDrop);
        // ADD MATERIAL VARIABLE HERE
        if (playerMaterial == null)
        {
            SaveSystem.currentSave.currentPlayerData.obtainedMaterials.Add(new CraftingMaterial(currentPlantDrop, 1));
        }
        else
        {
            playerMaterial.amount++;
        }

        if (hasReport)
        {
            GameObject tempReport = Instantiate(materialPrefab, materialPanel.transform);
            tempReport.transform.GetChild(1).GetComponent<TMP_Text>().text = plantData.plant[playerPlant].name + " Report";
        }
        SaveSystem.currentSave.currentPlayerData.currentPlant = -1;
        SaveSystem.currentSave.Save();
    }
}
