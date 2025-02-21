using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColour;

    private void Awake()
    {
        Deselect();
    }
    public void Select()
    {
        image.color = selectedColor;
    }
    public void Deselect()
    {
        image.color = notSelectedColour;
    }

    public void OnDrop(PointerEventData eventData){
        if(transform.childCount==0){
            InventorySkills inventorySkills = eventData.pointerDrag.GetComponent<InventorySkills>();
            inventorySkills.parentAfterDrag = transform;
        }
    }
}
