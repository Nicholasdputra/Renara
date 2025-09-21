using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantExtraction : MonoBehaviour
{
    public GameObject player;
    public GameObject gameObjectToDestroy;
    public GameObject extractionBackground;
    public Sprite[] extractionBackgroundSprites;
    public PlantSO plantToExtract;
    public PlantDataSO plantList;
    public bool doneExtracting = false;
    public int plantIndex = 0;

    public void EnablePlantExtraction()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(SaveSystem.currentSave.currentPlayerData.currentPlant == -1)
        {
            gameObject.GetComponentInChildren<PlantDisplay>().plant = plantToExtract;
            gameObject.GetComponentInChildren<PlantDisplay>().plantExtractionView = this;
            gameObject.SetActive(true);
        } 
        else{
            // Debug.Log("Plant already extracted, They can still move.");
            player.GetComponent<CharacterMovement>().canMove = true;
        }
            
    }

    public void DetermineWhatPlantToExtract(GameObject selectedPlant)
    {
        //determine based on selected plant name
        string plantName = selectedPlant.name;
        Debug.Log("Determining plant to extract: " + plantName);
        for(int i = 0; i < plantList.plant.Length; i++)
        {
            if (plantList.plant[i].plantName.ToLower() == plantName.ToLower())
            {
                plantToExtract = plantList.plant[i];
                plantIndex = i;
                if (plantName.ToLower().Contains("mutated"))
                {
                    extractionBackground.GetComponent<Image>().sprite = extractionBackgroundSprites[1];
                }
                else
                {
                    extractionBackground.GetComponent<Image>().sprite = extractionBackgroundSprites[0];
                }
                // EnablePlantExtraction();
                // player.GetComponent<CharacterMovement>().canMove = false;
                return;
            }
            else
            {
                Debug.Log("Didn't match with: " + plantList.plant[i].plantName);
            }
        }
    }

    public IEnumerator ClosePlantExtraction()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponentInChildren<PlantDisplay>().dialogueText.text = "";
        gameObject.GetComponentInChildren<PlantDisplay>().currentStep = 0;
        gameObject.GetComponentInChildren<PlantDisplay>().plant = null;
        gameObject.GetComponentInChildren<PlantDisplay>().plantExtractionView = null;
        if (gameObjectToDestroy != null)
        {
            Destroy(gameObjectToDestroy);
            // Debug.Log(gameObjectToDestroy.name + " has been destroyed.");
        }
        else
        {
            Debug.LogWarning("gameObjectToDestroy is null and cannot be destroyed.");
        }
        // Debug.Log(gameObjectToDestroy.name + " has been destroyed.");
        gameObject.SetActive(false);
        player.GetComponent<CharacterMovement>().canMove = true;
        SaveSystem.currentSave.currentPlayerData.currentPlant = plantIndex;
        SaveSystem.currentSave.Save();
    }
}
