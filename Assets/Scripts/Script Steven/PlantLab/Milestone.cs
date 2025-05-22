using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Milestone : MonoBehaviour
{
    public PlantDataSO plantData;
    public int plantsUnlocked = 0;
    [SerializeField] Slider slider;

    [ContextMenu ("SubmitPlant")]
    public void SubmitCurrentPlant(){
        Debug.Log("Submitting Plant");
        gameObject.SetActive(true);
        plantsUnlocked = 0;
        foreach(PlantSO plant in plantData.plant){
            if(plant.isUnlocked == true){
                plantsUnlocked++;
            }
        }
        slider.value = plantsUnlocked;

        //check if we have unlocked the plant or not
        int plantIndex = 0;
        if(plantData.plant[plantIndex].isUnlocked == false){
            //if not, unlock it
            plantData.plant[plantIndex].isUnlocked = true;
            plantsUnlocked++;
            Debug.Log("Plant Unlocked");
            UpdateMilestone();
        }
    }
    
    public void UpdateMilestone(){
        if(plantsUnlocked == 2){
            //call player tool level up
            //i assume the tool unlock will be handled by the player script?
            // Player.currentToolLevel++;
        }else if(plantsUnlocked == 4){
            // Player.currentToolLevel++;
        }else if(plantsUnlocked == 7){
            // Player.currentToolLevel++;
        }
        StartCoroutine("UpdateSlider");
    }

    IEnumerator UpdateSlider(){
        float t = 0;
        float duration = 2f;
        while(t < duration){
            t += Time.deltaTime;
            slider.value = plantsUnlocked-1 + t/duration;
            yield return null;
        }
        slider.value = plantsUnlocked;
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().canMove = true;
        gameObject.SetActive(false);
    }
}