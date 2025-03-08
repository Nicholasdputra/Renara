using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/PlantDataSO")]
public class PlantDataSO : ScriptableObject
{
    public PlantSO[] plantData;
    [Header("Insert Question Mark Plant Data Here")]
    public PlantSO unknownPlantData;
    public bool[] isUnlocked;
}