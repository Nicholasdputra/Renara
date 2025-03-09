using System.Collections;
using System.Collections.Generic;
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
    [Header ("Sequence References")]
    [SerializeField] DNAPanel dnaMatchingPanel;
    [SerializeField] DNAMatchingScript dnaGame;
    [SerializeField] TypingReportScript typingReportScript;
    [SerializeField] Milestone milestoneScript;
    [SerializeField] GameObject materialPanel;
    [SerializeField] GameObject materialPrefab;

    [ContextMenu("Start Lab")]
    public void StartLab()
    {
        // playerPlant = SaveSystem.currentSave.currentPlayerData.currentPlant;
        playerPlant = 0;
        hasConfirmed = false;
        hasExtracted = false;
        hasReport = false;
        gameObject.SetActive(true);
        plantImage.sprite = plantData.plant[playerPlant].plantImage;
        materialPanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !hasConfirmed && !hasExtracted)
        {
            hasConfirmed = true;
            dnaMatchingPanel.gameObject.SetActive(true);
            dnaGame.StartDNAExtraction();
        }else if(Input.GetKeyDown(KeyCode.Space) && hasExtracted){
            //close the lab
            gameObject.SetActive(false);
            for(int i = 1; i<materialPanel.transform.childCount; i++){
                Destroy(materialPanel.transform.GetChild(i).gameObject);
            }
            materialPanel.SetActive(false);
            if(hasReport){
                milestoneScript.SubmitCurrentPlant();
            }
        }
    }

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
            typingReportScript.StartReport();
        }
        else{
            //update plant extracted
            Debug.Log("Plant Already Unlocked");
            ShowExtractedMaterials(false);
        }
    }

    public void ShowExtractedMaterials(bool hasReport){
        this.hasReport = hasReport;
        Debug.Log("Showing Extracted Materials");
        hasExtracted = true;
        materialPanel.SetActive(true);
        GameObject material = Instantiate(materialPrefab, materialPanel.transform);
        // material.transform.GetChild(0).GetComponent<Image>().sprite = (MATERIAL SPRITE HERE);
        // material.transform.GetChild(1).GetComponent<TMP_Text>().text = (MATERIAL NAME HERE);
        // ADD MATERIAL VARIABLE HERE
        if(hasReport){
            GameObject tempReport = Instantiate(materialPrefab, materialPanel.transform);
            tempReport.GetComponent<TMP_Text>().text = plantData.plant[playerPlant].name + " Report";
        }
    }
}
