using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolDataSO", menuName = "ScriptableObjects/ToolDataSO")]
public class ToolDataSO : ScriptableObject
{
    public List<ToolSO> tools = new List<ToolSO>();
}
