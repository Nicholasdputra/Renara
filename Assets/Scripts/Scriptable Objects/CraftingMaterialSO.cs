using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingMaterialSO", menuName = "ScriptableObjects/CraftingMaterialSO")]
public class CraftingMaterialSO : ScriptableObject
{
    public string materialName;
    public string materialDescription;
    public Sprite materialSprite;
}

[System.Serializable]
public class CraftingMaterial
{
    public CraftingMaterialSO materialSO;
    public int amount;

    public CraftingMaterial(CraftingMaterialSO materialSO, int amount)
    {
        this.materialSO = materialSO;
        this.amount = amount;
    }
}

