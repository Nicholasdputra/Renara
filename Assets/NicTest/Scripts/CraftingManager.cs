using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public ItemDataSO ItemDataSO;
    public GameObject itemButtonPrefab;
    public GameObject itemButtonParent;
    public GameObject craftablePopUp;
    private List<Material> materialsNeeded;

    void OnAwake()
    {
        //Get materialsNeeded from ItemDataSO
        materialsNeeded = new List<Material>();
        foreach (ItemSO item in ItemDataSO.items)
        {
            foreach (Material material in item.materialsNeeded)
            {
                materialsNeeded.Add(material);
            }
        }

        //Determine which buttons to display based on the items in the player's inventory
            //you do this by comparing it to the recipes for every item
        //if it's craftable
            //Instantiate a button for that item
            //Set the button's text to the item's name
            //Set the icon to the item's icon
    }

    // Update is called once per frame
    void Update()
    {
        //If the player presses one of the buttons
            //Open the craftablePopUp
            //Set the image to the item's icon
            //Set the text to the item's name and description
            //Set the materials needed to the item's materials needed
    }
}
