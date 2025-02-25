using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    public MapScript mapScript;
    [Header("Put the location you want to teleport to here")]
    public Vector2 teleportLocation;
    public float playerZPos;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    public 
    void Teleport()
    {
        //calls the teleport function in the map script
        mapScript.Teleport(new Vector3(teleportLocation.x, teleportLocation.y, playerZPos));
        // Debug.Log("Teleporting to " + teleportLocation);
    }
}
