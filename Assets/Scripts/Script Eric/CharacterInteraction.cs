using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class CharacterInteraction : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private DialogueData dialogueData;

    [Header("Parameter")]
    [SerializeField] private bool typing;

    [Header("Object")]
    [SerializeField] private TMP_Text dialogueTextBox;
    [SerializeField] private GameObject dialogueBox;
    private void Start()
    {
        dialogueTextBox.text = "";
        dialogueBox.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            typing = true;
            dialogueBox.gameObject.SetActive(typing);
            DialogueSO dialogue = RetrieveDialogue("Welcoming");
            StopAllCoroutines();
            StartCoroutine(TypeDialogue(dialogue.dialogueText[UnityEngine.Random.Range(0, dialogue.dialogueText.Length)]));
        }
    }

    IEnumerator TypeDialogue(string sentence)
    {
        dialogueTextBox.text = "";
        Debug.Log("test");
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTextBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        typing = false;
        dialogueBox.gameObject.SetActive(typing);
    }

    DialogueSO RetrieveDialogue(string context)
    {
        DialogueSO dialogue = Array.Find(dialogueData.dialogues, x => x.name == context);
        return dialogue;
    }

}
