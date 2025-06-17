using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private Vector2 colliderOffset;
    [SerializeField] private Vector2 colliderSize;

    [SerializeField] private GameObject popUp;
    [SerializeField] private UnityEvent onInteract;

    [SerializeField] private PlayerDataSO playerData; //editan Rafa
    [SerializeField] private string cureName; //editan Rafa
    [SerializeField] private string targetCure; //editan Rafa


    private GameObject player;
    private bool interactable;
    private bool oneTimeDialogue;
    private bool dialogueIsDone;

    [Header("Dialogue")]
    [SerializeField] private string[] dialogues;

    private void Start()
    {
        interactable = false;
        if (gameObject.GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.GetComponent<BoxCollider2D>().size = colliderSize;
        gameObject.GetComponent<BoxCollider2D>().offset = colliderOffset;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E) && interactable)
        {
            if(gameObject.tag == "PlantToExtract")
            {
                if (GameObject.Find("Canvas") != null)
                {
                    // Debug.Log("Canvas found");
                    GameObject canvas = GameObject.Find("Canvas");
                    PlantExtraction plantExtraction = canvas.GetComponentInChildren<PlantExtraction>(true);

                    if (plantExtraction != null)
                    {
                        Debug.Log("PlantExtraction found");
                        Debug.Log("GameObject: " + gameObject.name);
                        Debug.Log("Current Overworld Plant: " + plantExtraction.name);
                        plantExtraction.DetermineWhatPlantToExtract(gameObject);
                        plantExtraction.gameObjectToDestroy = gameObject;
                    }
                    else
                    {
                        Debug.Log("PlantExtraction not found");
                        // PlantExtraction newPlantExtraction = canvas.AddComponent<PlantExtraction>();
                        // newPlantExtraction.gameObjectToDestroy = gameObject;
                    }
                }
            }
            onInteract.Invoke();
        }
    }

    public void OneTimeDialogue(bool status)
    {
        oneTimeDialogue = status;
    }

    public void CallRandomDialogue()
    {
        if (dialogueIsDone && oneTimeDialogue)
        {
            player.GetComponent<CharacterMovement>().canMove = true;

            return;
        }
        player.GetComponent<CharacterInteraction>().dialogues = dialogues;
        player.GetComponent<CharacterInteraction>().PlayRandomDialogue();
    }

    public void CallFullDialogue()
    {
        if (dialogueIsDone && oneTimeDialogue)
        {
            player.GetComponent<CharacterMovement>().canMove = true;

            return;
        }
        
        player.GetComponent<CharacterInteraction>().dialogues = dialogues;
        player.GetComponent<CharacterInteraction>().PlayDialogue();
    }

    public void StopCharacterMovement()
    {
        player.GetComponent<CharacterMovement>().canMove = false;
        // Debug.Log(dialogueIsDone);
        // Debug.Log(oneTimeDialogue);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        popUp.SetActive(true);
        interactable = true;
        dialogueIsDone = collision.gameObject.GetComponent<CharacterInteraction>().isDialogueDone;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        popUp.SetActive(false);
        interactable = false;
    }

    public void UseCure()
    {
        UseCureAndDestroyTaggedObjects(targetCure, cureName); 
    }

    public void UseCureAndDestroyTaggedObjects(string tagToDestroy, string cureName) //kodenya rafa
    {
        if (playerData != null && playerData.obtainedItemDataSO != null)
        {
            bool hasCure = false;

            foreach (var item in playerData.obtainedItemDataSO.items)
            {
                if (item.itemName == cureName) 
                {
                    hasCure = true;
                    break;
                }
            }

            if (hasCure)
            {
                //it would be great kalo ada function blackscreen disini biar ngeblock random dialog, bingung gimana cara ngilanginnya

                GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tagToDestroy);

                foreach (GameObject obj in taggedObjects)
                {
                    Destroy(obj);
                }

                Debug.Log("Cure used. Destroyed all objects with tag: " + tagToDestroy);
            }
            else
            {
                Debug.Log("Player does not have the cure.");
            }
        }
        else
        {
            Debug.Log("Player data or obtained item data is missing.");
        }
    }


}
