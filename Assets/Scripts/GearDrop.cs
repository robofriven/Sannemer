using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class GearDrop : MonoBehaviour, IDropHandler
{
    public Text textField;

    private bool occupied;
    

    public void OnDrop(PointerEventData eventData)
    {
        GearDrag gear = eventData.pointerDrag.GetComponent<GearDrag>();

        if (this.transform.childCount != 0)
            occupied = true;
        else
            occupied = false;

        if (gear == null)
            return;

        if (!occupied)
        {
            gear.parentToReturnTo = this.transform;

            //assign attack and/or defense text
            textField.text = gear.GetComponent<Gear>().strength.ToString();

        }
    }
}
