using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAPanel : MonoBehaviour
{
    //This script is only used because the DNA sliding script needs to be in the same level as the scrollrect
    // So this is used to close the panel only
    
    [SerializeField] LabManager labManager;
    public void CloseWindow(){
        // Debug.Log("This is the final output for this minigame, call ur next functions from here");
        labManager.CloseDNAWindow();
    }
}
