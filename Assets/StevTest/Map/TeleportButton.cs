using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    public MapScript mapScript;
    [Header("Put the location you want to teleport to here")]
    public Vector3 teleportLocation;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    public 
    void Teleport()
    {
        //calls the teleport function in the map script
        mapScript.Teleport(teleportLocation);
        // Debug.Log("Teleporting to " + teleportLocation);
    }
}
