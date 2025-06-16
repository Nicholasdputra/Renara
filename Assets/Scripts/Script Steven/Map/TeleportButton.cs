using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeleportButton : MonoBehaviour
{
    public MapScript mapScript;
    public bool teleportToLab;
    [Header("Put the location you want to teleport to here")]
    public Vector2 teleportLocation;
    float playerZPos;

    void Start()
    {
        if(mapScript == null)
        {
            mapScript = GameObject.Find("MapManager").GetComponent<MapScript>();
        }
        playerZPos = GameObject.FindGameObjectWithTag("Player").transform.position.z;
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    public 
    void Teleport()
    {
        if (!teleportToLab)
        {
            if (SceneManager.GetActiveScene().name == "Lab")
            {
                Debug.Log("Teleporting to Overworld");
                Debug.Log("Teleport Location: " + teleportLocation);
                SaveSystem.currentSave.currentPlayerData.position = new Vector3(teleportLocation.x, teleportLocation.y, playerZPos);
                Debug.Log("Player Position Set to: " + SaveSystem.currentSave.currentPlayerData.position);
                SaveSystem.currentSave.Save();
                SceneManager.LoadScene("Overworld");
                return;
            }
            //calls the teleport function in the map script
            mapScript.Teleport(new Vector3(teleportLocation.x, teleportLocation.y, playerZPos));
            // Debug.Log("Teleporting to " + teleportLocation);
        }
        else
        {
            SceneManager.LoadScene("Lab");
        }
        SaveSystem.currentSave.Save();
    }
}
