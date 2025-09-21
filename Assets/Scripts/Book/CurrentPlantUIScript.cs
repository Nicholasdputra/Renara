using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlantUIScript : MonoBehaviour
{
    PlayerDataSO playerData;
    Image plant;
    // Start is called before the first frame update
    void Start()
    {
        playerData = SaveSystem.currentSave.currentPlayerData;
        plant = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerData.currentPlant == -1)
        {
            plant.sprite = null;
            plant.color = new Color(1, 1, 1, 0); // Make the plant image invisible
        }
        else
        {
            plant.sprite = playerData.plantDataSO.plant[playerData.currentPlant].plantImage;
            plant.color = new Color(1, 1, 1, 1); // Make the plant image visible
        }
    }
}
