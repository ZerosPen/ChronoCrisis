using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public GameObject inventorySkillsPrefab;
    int selectedSlot = -1;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSelectedSlot (0);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeSelectedSlot (1);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeSelectedSlot (2);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeSelectedSlot (3);
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >=0)
        {
            skillSlots[selectedSlot].Deselect();
        }
        skillSlots[newValue].Select();
        selectedSlot = newValue;
    }


    public bool AddItem(AoE items)
    {
        for(int i=0; i< skillSlots.Length ;i++)
        {
            SkillSlot slot = skillSlots[i];
            InventorySkills itemInSlot = slot.GetComponentInChildren<InventorySkills>();
            // if (itemInSlot != null && itemInSlot.items==items && itemInSlot.count < maxStackedItems && itemInSlot.items.stackable == true )
            // {
            //     itemInSlot.count++;
            //     itemInSlot.RefreshCount();
            //     return true;
            // }
        }

        for(int i=0; i< skillSlots.Length ;i++)
        {
            SkillSlot slot = skillSlots[i];
            InventorySkills itemInSlot = slot.GetComponentInChildren<InventorySkills>();
            if (itemInSlot == null)
            {
                SpawnNewItem(items,slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem (AoE items, SkillSlot slot)
    {
        GameObject newItemGo = Instantiate(inventorySkillsPrefab, slot.transform);
        InventorySkills inventorySkills = newItemGo.GetComponent<InventorySkills>();
        inventorySkills.InitialiseItem(items);
    }



}
