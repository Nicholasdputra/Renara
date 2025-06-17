using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "ScriptableObjects/Plant")]
public class PlantSO : ScriptableObject
{
    public Sprite plantImage;
    public string plantName;
    public string plantDescription;
    public string[] extractionSteps;
    public bool isUnlocked;
    public ReportSO report;
    public CraftingMaterialSO materialDrop;

    public PlantSO(Sprite plantImage, string plantName, string plantDescription, string[] extractionSteps, bool isUnlocked){
        this.plantImage = plantImage;
        this.plantName = plantName;
        this.plantDescription = plantDescription;
        this.extractionSteps = extractionSteps;
        this.isUnlocked = false;
    }
}