using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "ScriptableObjects/Tool")]
public class ToolSO : ScriptableObject
{
    public string toolName;
    public string toolDescription;
    public Sprite toolSprite;
}
