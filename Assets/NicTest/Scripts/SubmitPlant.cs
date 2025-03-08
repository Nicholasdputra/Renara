using UnityEngine;

public class SubmitPlant : MonoBehaviour
{
    public PlantDataSO plantData;
    public int plantsUnlocked = 0;
    // Start is called before the first frame update
    void Start()
    {
        plantsUnlocked = 0;
        foreach(PlantSO plant in plantData.plantData){
            if(plant.isUnlocked == true){
                plantsUnlocked++;
            }
        }
    }

    // public void Interact(){
    //     //Load
    //     plantsUnlocked = 0;
    //     foreach(PlantSO plant in plantData.plantData){
    //         if(plant.isUnlocked == true){
    //             plantsUnlocked++;
    //         }
    //     }

    //     if(SaveSystem.currentSave.currentPlayerData.currentPlant != -1){
    //         SubmitCurrentPlant(SaveSystem.currentSave.currentPlayerData.currentPlant);
    //     }
    // }

    public void SubmitCurrentPlant(int plantIndex){
        //check if we have unlocked the plant or not
        if(plantData.plantData[plantIndex].isUnlocked == false){
            //if not, unlock it
            plantData.plantData[plantIndex].isUnlocked = true;
            plantsUnlocked++;
            Debug.Log("Plant Unlocked");
            UpdateMilestone();
        }
        else{
            //update plant extracted
            Debug.Log("Plant Already Unlocked");
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
    }
}