using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;

    [SerializeField] private CharacterInteraction charaInteract;
    private void Start()
    {
        charaInteract = GameObject.FindObjectOfType<CharacterInteraction>();
    }

    public void ChooseSentence(string context)
    {
        DialogueSO dialogue = RetrieveDialogue(context);
        int randomIndex = UnityEngine.Random.Range(0, dialogue.dialogueText.Length);
        StopAllCoroutines();
        StartCoroutine(charaInteract.TypeDialogue(dialogue.dialogueText[randomIndex]));
    }

    private DialogueSO RetrieveDialogue(string context)
    {
        DialogueSO dialogue = Array.Find(dialogueData.dialogues, x => x.name == context);
        return dialogue;
    }
}
