using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ScriptableObjects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public ItemSO[] items;
}
