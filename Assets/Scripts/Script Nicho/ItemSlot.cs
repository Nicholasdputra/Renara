using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public bool isOccupied = false;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        //kl ada yang di drag ke item slot tertentu yang kosong
        if (eventData.pointerDrag != null && isOccupied == false)
        {
            //set the item's position to the item slot's position and set the item slot to occupied
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<DragAndDropScript>().inItemSlot = true;
            
            //kl set isOccupiednya item slot sebelumnya jadi false
            if(eventData.pointerDrag.GetComponent<DragAndDropScript>().currentItemSlot != null){
                eventData.pointerDrag.GetComponent<DragAndDropScript>().currentItemSlot.isOccupied = false; 
            }

            //set the item slot of the item to this item slot and set the item slot to occupied
            eventData.pointerDrag.GetComponent<DragAndDropScript>().currentItemSlot = this;
            isOccupied = true;
        }
    }
}
