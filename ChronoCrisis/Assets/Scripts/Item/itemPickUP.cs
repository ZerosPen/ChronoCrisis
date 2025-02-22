using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickUP : MonoBehaviour
{
    public Item itemData; // Assign the ScriptableObject in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemData.UseItem(other.gameObject);
            Destroy(gameObject); // Remove the item after use
        }
    }
}
