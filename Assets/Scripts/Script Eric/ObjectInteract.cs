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
                    if (canvas.GetComponentInChildren<PlantExtraction>() != null)
                    {
                        // Debug.Log("PlantExtraction found");
                        PlantExtraction currentOverworldPlant = canvas.GetComponentInChildren<PlantExtraction>();
                        currentOverworldPlant.gameObjectToDestroy = gameObject;
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
        if (dialogueIsDone && oneTimeDialogue) return;
        player.GetComponent<CharacterInteraction>().dialogues = dialogues;
        player.GetComponent<CharacterInteraction>().PlayRandomDialogue();
    }

    public void CallFullDialogue()
    {
        if (dialogueIsDone && oneTimeDialogue) return;
        
        player.GetComponent<CharacterInteraction>().dialogues = dialogues;
        player.GetComponent<CharacterInteraction>().PlayDialogue();
    }

    public void StopCharacterMovement()
    {
        player.GetComponent<CharacterMovement>().canMove = false;
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
}
