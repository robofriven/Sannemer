using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class GearDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Gear gearPrefab;

    public GameObject gearField;
    public GameObject[] gearPlacement;
    public Transform parentToReturnTo;
    private Gear newGear;
    private int siblingIndex;
    private GameObject gearPanel;
    private bool proceed = true;

    void Start()
    {
        // Define gearField
        gearField = GameObject.FindWithTag("Gear Field");
        // Define array gearplacement
        gearPlacement = GameObject.FindGameObjectsWithTag("Gear Placement");
        gearPanel = GameObject.FindGameObjectWithTag("Gear UI Panel");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On Begin Drag");
        if (this.transform.parent.name != "Gears")
        {
            Debug.Log("Doesn't belong to the field");
            proceed = false;
            return;
        }
        Debug.Log(this.transform.parent.name);

        parentToReturnTo = gearPanel.transform;
        //siblingIndex = this.transform.GetSiblingIndex();
        GetComponent<CanvasGroup>().blocksRaycasts = false;


        // Create a gear to replace the one that was just picked up
        newGear = GameObject.Instantiate(gearPrefab, this.transform.position, Quaternion.identity) as Gear;
        newGear.GetComponent<CanvasGroup>().alpha = 0;
        newGear.transform.SetParent(parentToReturnTo);
        newGear.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (proceed)
            newGear.transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (proceed)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (parentToReturnTo != gearPanel.transform)
            {
                //Debug.Log("On End Drag and Parent = this parent");
                newGear.transform.SetParent(parentToReturnTo);
            }
            else
                Destroy(newGear.gameObject);
        }
    }
}
