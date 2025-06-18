using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropScript : MonoBehaviour, 
IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public bool inItemSlot = true;
    public Vector2 startingPosition;
    public ItemSlot currentItemSlot;
    [SerializeField] Canvas canvas;
    private void Awake()
    {
        canvas = transform.parent.parent.GetComponent<Canvas>();
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        inItemSlot = false;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //At the end of the drag, check if the item is in an item slot, 
        //Wait for a sec first tho so that the itembox has time to update
        StartCoroutine(CheckItemSlot());
        
    }

    private IEnumerator CheckItemSlot()
    {
        // Wait for the end of the frame to ensure the item slot check is completed
        yield return new WaitForEndOfFrame();

        if (inItemSlot == false)
        {
            rectTransform.anchoredPosition = startingPosition;
        }
        else
        {
            startingPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }
}
