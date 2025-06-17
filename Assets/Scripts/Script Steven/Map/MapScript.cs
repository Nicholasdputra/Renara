using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    CharacterMovement characterMovement;
    public bool canOpenMap;
    [SerializeField] GameObject mapPanel;

    void Start()
    {
        mapPanel.SetActive(false);
        characterMovement = player.GetComponent<CharacterMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        canOpenMap = characterMovement.canMove;
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (canOpenMap||mapPanel.activeSelf)
            {
                ToggleMap();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && mapPanel.activeSelf)
        {
            ToggleMap();
        }

        if (mapPanel.activeSelf)
        {
            characterMovement.canMove = false;
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
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        mapPanel.SetActive(false);
        characterMovement.canMove = true;
        //teleport fade in animation
    }
}
