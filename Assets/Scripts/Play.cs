using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Play : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool occupied = false;

    public GameObject handPanel;



    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(occupied);
        //Debug.Log(this.transform.childCount);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (this.transform.childCount != 0)
            occupied = true;
        else
            occupied = false;

        if (d == null)
            return;

        if (!occupied)
        {
            d.parentToReturnTo = this.transform;
        }
        else
        {
            d.parentToReturnTo = handPanel.transform;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)                                  // if not dragging you can't do anything so return immediately
            return;

        //Debug.Log("On Point Enter called");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)                                  // if not dragging you can't do anything so return immediately
            return;

        if (this.transform.childCount != 0)
            occupied = true;
        else
            occupied = false;

        //print("On Pointer Exit");
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)             
            d.placeholderParent = d.parentToReturnTo;                       
    }
}
