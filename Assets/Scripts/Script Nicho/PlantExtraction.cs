using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantExtraction : MonoBehaviour
{
    public GameObject player;
    public GameObject gameObjectToDestroy;
    public PlantSO plantToExtract;
    public bool doneExtracting = false;

    public void EnablePlantExtraction()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
    }
}
