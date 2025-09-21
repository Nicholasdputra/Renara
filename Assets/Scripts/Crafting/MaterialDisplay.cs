using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialDisplay : MonoBehaviour
{
    public CraftingMaterialSO craftingMaterial;
    public Image materialImage;
    public string materialName;
    public string materialDescription;
    public Sprite materialSprite;
    public Image image;

    void Start()
    {
        name = craftingMaterial.materialName;
        materialName = craftingMaterial.materialName;
        materialDescription = craftingMaterial.materialDescription;
        materialSprite = craftingMaterial.materialSprite;
        image = GetComponent<Image>();
        image.sprite = materialSprite; 
    }
}