using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ScriptableObjects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();

    [ContextMenu("Clear Items")]
    public void ClearItems()
    {
        items = new List<ItemSO>();
    }
}