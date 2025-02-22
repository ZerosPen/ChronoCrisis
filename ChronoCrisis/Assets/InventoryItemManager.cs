using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemManager : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public GameObject inventoryItemssPrefab;
    public int maxStackedItems=69;


    int selectedSlot = -1;


    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         ChangeSelectedSlot (0);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         ChangeSelectedSlot (1);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ChangeSelectedSlot (2);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.T))
    //     {
    //         ChangeSelectedSlot (3);
    //     }
    // }

    // void ChangeSelectedSlot(int newValue)
    // {
    //     if(selectedSlot >=0)
    //     {
    //         itemSlots[selectedSlot].Deselect();
    //     }
    //     itemSlots[newValue].Select();
    //     selectedSlot = newValue;
    // }


    public bool AddItem2(Items items)
    {
        for(int i=0; i< itemSlots.Length ;i++)
        {
            ItemSlot slot = itemSlots[i];
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot != null && itemInSlot.items==items && itemInSlot.count < maxStackedItems && itemInSlot.items.stackable == true )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for(int i=0; i< itemSlots.Length ;i++)
        {
            ItemSlot slot = itemSlots[i];
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot == null)
            {
                SpawnNewItem2(items,slot);
                return true;
            }
        }
        return false;
    }

        void SpawnNewItem2 (Items items, ItemSlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemssPrefab, slot.transform);
        InventoryItems inventoryItems = newItemGo.GetComponent<InventoryItems>();
        inventoryItems.InitialiseItem(items);
    }
}
