using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image image;
        public void OnDrop(PointerEventData eventData){
        if(transform.childCount==0){
            InventoryItems InventoryItems = eventData.pointerDrag.GetComponent<InventoryItems>();
            InventoryItems.parentAfterDrag = transform;
        }
    }
}
