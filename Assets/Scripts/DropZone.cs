using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    bool attack = false;
    bool defense = false;

    public void OnDrop(PointerEventData eventData)
    {


        //print(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
            //print("Dropped on Attack and bool is " + attack);

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
            {
                d.parentToReturnTo = this.transform;
            }
    }
    
    // Changes the placeholder parent to the drop zone that it's hovered over
    public void OnPointerEnter(PointerEventData eventData)
    {
        //print("On PointerEnter");
        if (eventData.pointerDrag == null)                                  // if not dragging you can't do anything so return immediately
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }


    // When exiting the zone you were just in it changes the placholder back to the original parent
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)                                  // if not dragging you can't do anything so return immediately
            return;
        //print("On Pointer Exit");
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)             // the && is in for error checking, if you are exiting a different zone this shouldn't fire
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }
}
