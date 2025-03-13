using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class CharacterInteraction : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private bool isTalking;

    [Header("Object")]
    [SerializeField] private TMP_Text dialogueTextBox;
    [SerializeField] private GameObject dialogueBox;
    private void Start()
    {
        isTalking = false;
        dialogueTextBox.text = "";
        dialogueBox.gameObject.SetActive(false);
    }
    
    public IEnumerator TypeDialogue(string sentence)
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
            dialogueTextBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        isTalking = false;
        dialogueBox.gameObject.SetActive(false);
    }
}
