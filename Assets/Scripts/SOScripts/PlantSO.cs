using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "ScriptableObjects/Plant")]
public class PlantSO : ScriptableObject
{
    public Sprite plantImage;
    public Sprite mutatedImage;
    public string plantName;
    public string plantDescription;
    public string[] extractionSteps;
    public bool isUnlocked;
    public CraftingMaterialSO materialDrop;

    public PlantSO(Sprite plantImage, Sprite mutatedImage, string plantName, string plantDescription, string[] extractionSteps, bool isUnlocked){
        this.plantImage = plantImage;
        this.mutatedImage = mutatedImage;
        this.plantName = plantName;
        this.plantDescription = plantDescription;
        this.extractionSteps = extractionSteps;
        this.isUnlocked = false;
    }
}