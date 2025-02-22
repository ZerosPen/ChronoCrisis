using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
        public void OnDrop(PointerEventData eventData){
        if(transform.childCount==0){
            InventorySkills inventorySkills = eventData.pointerDrag.GetComponent<InventorySkills>();
            inventorySkills.parentAfterDrag = transform;
        }
    }
}
