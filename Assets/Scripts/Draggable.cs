using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;

    GameObject placeholder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("On Begin Drag called");

        // Build a placeholder 'card' to add a gap
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        // Sibling Index is the position it is in among the other 'siblings' under the parent
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());


        // Remember the original parent of the card incase the drop spot is invalid
        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag called");
        this.transform.position = eventData.position;

        if (placeholder.transform.parent != placeholderParent)
            placeholder.transform.SetParent(placeholderParent);

        // Default putting the card to the far right.  Fixes the "never to the left" problem of the loop
        int newSiblingIndex = placeholderParent.childCount;

        // Loop through the cards to see where the placeholder card should be
        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                    break;
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("On End Drag");
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);

        

    }
}
