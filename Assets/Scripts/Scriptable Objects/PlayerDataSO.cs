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
    public bool hasCuredZone1;
    public bool hasCuredZone2;
    public bool hasCuredZone3;
    public bool hasCuredZone4;

    [ContextMenu("Clear Data")]
    public void ClearData()
    {
        obtainedItemDataSO.ClearItems();
        plantDataSO.ClearData();
        obtainedMaterials = new List<CraftingMaterial>();
        currentPlant = -1;
        playerToolLevel = 0;
        hasCuredZone1 = false;
        hasCuredZone2 = false;
        hasCuredZone3 = false;
        hasCuredZone4 = false;
    }
}