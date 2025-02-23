using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSkillScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public SkillManager skillManager; // Reference to SkillManager
    public AoE[] skillsToPurchase; // Array of skills available in the shop

    public void PickupItem(int id)
    {
        if (id < 0 || id >= skillsToPurchase.Length)
        {
            Debug.LogError("Invalid skill ID!");
            return;
        }

        AoE skill = skillsToPurchase[id];

        if (inventoryManager != null)
        {
            inventoryManager.AddItem(skill);
            Debug.Log($"Skill {skill.name} added to inventory.");
        }
        else
        {
            Debug.LogError("InventoryManager is missing!");
        }

        if (skillManager != null)
        {
            skillManager.AddSkill(skill);
            Debug.Log($"Skill {skill.name} added to SkillManager.");
        }
        else
        {
            Debug.LogError("SkillManager is missing!");
        }
    }
}
