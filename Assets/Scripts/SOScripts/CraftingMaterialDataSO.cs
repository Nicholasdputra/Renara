using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingMaterialDataSO", menuName = "ScriptableObjects/CraftingMaterialDataSO")]
public class MaterialDataSO : ScriptableObject
{
    public CraftingMaterialSO[] craftingMaterials;
}
