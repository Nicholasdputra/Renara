using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantExtraction : MonoBehaviour
{
    public GameObject gameObjectToDestroy;
    public PlantSO plantToExtract;
    public bool doneExtracting = false;

    public void EnablePlantExtraction()
    {
        gameObject.GetComponentInChildren<PlantDisplay>().plant = plantToExtract;
        gameObject.GetComponentInChildren<PlantDisplay>().plantExtractionView = this;
        gameObject.SetActive(true);
    }

    public IEnumerator ClosePlantExtraction()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponentInChildren<PlantDisplay>().dialogueText.text = "";
        gameObject.GetComponentInChildren<PlantDisplay>().currentStep = 0;
        gameObject.GetComponentInChildren<PlantDisplay>().plant = null;
        gameObject.GetComponentInChildren<PlantDisplay>().plantExtractionView = null;
        Destroy(gameObjectToDestroy);
        gameObject.SetActive(false);
        
    }
}
