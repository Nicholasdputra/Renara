using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneLoader : MonoBehaviour
{
    void Start()
    {
        PlayerDataSO playerData = SaveSystem.currentSave.currentPlayerData;
        if (playerData.hasCuredZone1)
        {
            Debug.Log("Zone 1 cured, deactivating objects with tag Zone1");
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Zone1");

            foreach (GameObject obj in taggedObjects)
            {
                obj.SetActive(false); // Deactivate the object instead of destroying it
            }
        }

        if (playerData.hasCuredZone2)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Zone2");

            foreach (GameObject obj in taggedObjects)
            {
                obj.SetActive(false); // Deactivate the object instead of destroying it
            }
        }

        if (playerData.hasCuredZone3)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Zone3");

            foreach (GameObject obj in taggedObjects)
            {
                obj.SetActive(false); // Deactivate the object instead of destroying it
            }
        }
        
        if (playerData.hasCuredZone4)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Zone4");

            foreach (GameObject obj in taggedObjects)
            {
                obj.SetActive(false); // Deactivate the object instead of destroying it
            }
        }
    }
}
