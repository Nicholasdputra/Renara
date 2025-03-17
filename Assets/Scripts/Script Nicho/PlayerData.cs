using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    public ItemDataSO obtainedItemDataSO;
    public ItemDataSO listOfRecipes;
    public ToolDataSO toolDataSO;
    public PlantDataSO plantDataSO;
    [SerializeField] public List<CraftingMaterial> obtainedMaterials;
    public Vector3 position;
    public int currentPlant;
    public int playerToolLevel;
}