using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Itemss[] itemsToPickup;

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(itemsToPickup[id]);
    }
}
