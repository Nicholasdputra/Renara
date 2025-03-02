using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Item item;
    public Image image;

    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public string[] materialsNeeded;

    void Start()
    {
        name = item.itemName;
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        itemSprite = item.itemSprite;
        materialsNeeded = item.materialsNeeded;
        image = GetComponent<Image>();
        image.sprite = itemSprite; 
    }
}
