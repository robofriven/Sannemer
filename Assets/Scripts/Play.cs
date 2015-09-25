using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Play : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool occupied = false;

    public GameObject handPanel;
    public Text AttText;
    public Text DefText;


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
            //Debug.Log("Not Occupied");
            d.parentToReturnTo = this.transform;
            if (d.GetComponent<Gear>() != null)
            {
                //Debug.Log("There's a gear!");
                var gear = d.GetComponent<Gear>();
                if (this.transform.name == "Attack Gear Spot")
                {
                    AttText.text = gear.strength.ToString();
                }
                else if (this.transform.name == "Defense Gear Spot")
                {
                    DefText.text = gear.strength.ToString();
                }
            }
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

        //Debug.Log("On Pointer Enter called");
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
