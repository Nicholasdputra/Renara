using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    public bool canOpenMap;
    [SerializeField] GameObject mapPanel;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(canOpenMap)
            {
                ToggleMap();
            }
        }
    }

    public void ToggleMap()
    {
        mapPanel.SetActive(!mapPanel.activeSelf);
    }

    public void Teleport(Vector3 location)
    {
        //teleport fade out animation
        //teleport player and camera to location
        player.transform.position = location;
        Camera.main.transform.position = player.transform.position + Camera.main.transform.GetComponent<CameraFollow>().offset;
        mapPanel.SetActive(false);
        //teleport fade in animation
    }
}
