using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Plant")]
public class PlantSO : ScriptableObject
{
    public Sprite plantImage;
    public Sprite mutatedImage;
    public string plantName;
    public string plantDescription;
    public int[] extractionSteps;

    public PlantSO(Sprite plantImage, Sprite mutatedImage, string plantName, string plantDescription, int[] extractionSteps){
        this.plantImage = plantImage;
        this.mutatedImage = mutatedImage;
        this.plantName = plantName;
        this.plantDescription = plantDescription;
        this.extractionSteps = extractionSteps;
    }
}