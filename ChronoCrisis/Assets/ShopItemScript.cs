using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemScript : MonoBehaviour
{
    public InventoryItemManager inventoryItemManager;
    public Items[] itemsToPickup;

    public void PickupItem2(int id)
    {
        inventoryItemManager.AddItem2(itemsToPickup[id]);
    }
}
