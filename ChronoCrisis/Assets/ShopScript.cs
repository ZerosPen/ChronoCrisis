using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Items[] itemsToPickup;

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(itemsToPickup[id]);
    }
    public Items[] itemsToPickup2;

    public void PickupItem2(int id)
    {
        inventoryManager.AddItem2(itemsToPickup[id]);
    }
}
