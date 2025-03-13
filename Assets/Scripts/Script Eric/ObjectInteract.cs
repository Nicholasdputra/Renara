using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private Vector2 colliderOffset;
    [SerializeField] private Vector2 colliderSize;

    [SerializeField]private GameObject popUp;
    private bool interactable;
    [SerializeField] private UnityEvent onInteract;

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
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E) && interactable)
        {
            onInteract.Invoke();
            Debug.Log("Interacted");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        popUp.SetActive(true);
        interactable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        popUp.SetActive(false);
        interactable = false;
    }
}
