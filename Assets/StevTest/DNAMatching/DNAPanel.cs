using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAPanel : MonoBehaviour
{
    
    public void CloseWindow(){
        Debug.Log("This is the final output for this minigame, call ur next functions from here");
        gameObject.SetActive(false);
        
    }
}
