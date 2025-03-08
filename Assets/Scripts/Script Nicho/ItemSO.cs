using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public Material[] materialsNeeded;

}

[System.Serializable]
public class Material
{
    public string materialName;
    public int materialAmount;
}