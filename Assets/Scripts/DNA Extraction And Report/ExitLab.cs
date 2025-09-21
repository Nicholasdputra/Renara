using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLab : MonoBehaviour
{
    [SerializeField] MapScript mapScript;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().canMove = false;
        SaveSystem.currentSave.currentPlayerData.position = new Vector3(-3, 1, 0);
        Debug.Log("Player Position Set to: " + SaveSystem.currentSave.currentPlayerData.position);
        SaveSystem.currentSave.Save();
        TransisionScript.Transision("Overworld");
    }
}
