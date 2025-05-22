using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class CharacterInteraction : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private bool isTalking;
    [SerializeField] private bool isSkiping;

    [Header("Object")]
    [SerializeField] private TMP_Text dialogueTextBox;
    [SerializeField] private GameObject dialogueBox;

    [Header("CurrentDialogue")]
    [SerializeField] private int currentDialogueIndex;
    public bool isDialogueDone { get; private set; }
    public string[] dialogues;

    private void Start()
    {
        isTalking = false;
        dialogueTextBox.text = "";
        dialogueBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(isTalking)
            {
                if (currentDialogueIndex <= dialogues.Length - 1)
                {
                    isSkiping = true;
                }
            }
            Debug.Log(isTalking);
            Debug.Log(isSkiping);
        }
    }
    
    public IEnumerator TypeDialogue(string sentence, bool isRand)
    {
        if (isTalking)
        {
            yield return null;
        }

        isTalking = true;
        dialogueTextBox.text = "";
        dialogueBox.gameObject.SetActive(true);

        foreach (char letter in sentence.ToCharArray())
        {
            if (isSkiping)
            {
                dialogueTextBox.text = sentence;
                isSkiping = false;  
                break;
            }
            dialogueTextBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTalking = false;
        yield return new WaitForSeconds(2f);
        currentDialogueIndex++;

        if (isRand)
        {
            currentDialogueIndex = 0;
            dialogueBox.gameObject.SetActive(false);
            dialogueTextBox.text = "";
            isTalking = false;
            gameObject.GetComponent<CharacterMovement>().canMove = true;
            isDialogueDone = true;
        }
        else if (currentDialogueIndex <= dialogues.Length - 1)
        {
            PlayDialogue();
        }
        else
        {
            currentDialogueIndex = 0;
            dialogueBox.gameObject.SetActive(false);
            dialogueTextBox.text = "";
            isTalking = false;
            gameObject.GetComponent<CharacterMovement>().canMove = true;
            isDialogueDone = true;
        }
    }

    public void PlayDialogue()
    {
        if (isTalking) return;

        StopAllCoroutines();
        //gameObject.GetComponent<CharacterMovement>().canMove = false;
        StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex], false));
    }

    public void PlayRandomDialogue()
    {
        if (isTalking) return;

        StopAllCoroutines();
        //gameObject.GetComponent<CharacterMovement>().canMove = false;
        int randomIndex = UnityEngine.Random.Range(0, dialogues.Length);
        StartCoroutine(TypeDialogue(dialogues[randomIndex], true));
    }
}
