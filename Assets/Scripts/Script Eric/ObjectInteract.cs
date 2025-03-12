using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;
    private bool interactable;

    private void Start()
    {
        interactable = false;
        if(gameObject.GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && interactable)
        {
            Debug.Log("OMG ITS INTERACTING");
            onInteract.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        interactable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
    }
}
