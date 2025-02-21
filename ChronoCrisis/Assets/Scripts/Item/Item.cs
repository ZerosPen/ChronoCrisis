using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/item")]

public class Item : Items
{
    public override void UseItem(GameObject Player)
    {
        PlayerController playerStatus = Player.GetComponent<PlayerController>();
        if(playerStatus == null)
        {
            Debug.LogError("PlayerController component not found on Player!");
            return;
        }

        if(itemType == "SuperRecover")
        {
            playerStatus.RestoreStatus(valueItem);
        }
        if(itemType == "RecoverMana")
        {
            playerStatus.RestoreStatus(valueItem);
        }
        if (itemType == "RecoverHp")
        {
            playerStatus.RestoreStatus(valueItem);
        }
    }
}
