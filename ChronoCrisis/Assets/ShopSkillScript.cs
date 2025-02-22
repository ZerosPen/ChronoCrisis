using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSkillScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public AoE[] itemsToPickup;

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(itemsToPickup[id]);
    }

}
