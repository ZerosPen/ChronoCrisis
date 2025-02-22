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

        if(itemType == "PowerUp")
        {
            Debug.Log($"PowerUp item detected: {itemName}");
            switch (itemName)
            {
                case "BlueBerry":
                    Debug.Log("Eating Berrry");
                    playerStatus.manaPoint += 5f;
                    playerStatus.magicPower += 2.5f;
                    Debug.Log($"currManaPoint : {playerStatus.currManaPoint}");
                    Debug.Log($"ManaPoint : {playerStatus.manaPoint}");
                    Debug.Log($"MagicPower : {playerStatus.magicPower}");
                    break;

                case "GreenBerry":
                    Debug.Log($"UseItem() called for {itemName}");


                    float speedBoost = playerStatus.movementSpeed * (playerStatus.pointAgi / 200f);
                    float AttackSpeedUp = playerStatus.AttackSpeed * (playerStatus.pointAgi / 200f);

                    speedBoost = Mathf.Max(speedBoost, 0.1f);  // Prevent zero increase
                    AttackSpeedUp = Mathf.Max(AttackSpeedUp, 0.1f);

                    playerStatus.movementSpeed += speedBoost;
                    playerStatus.AttackSpeed += AttackSpeedUp;

                    break;

                case "RedBerry":
                    Debug.Log("Eating Berrry");
                    playerStatus.damageATK++;
                    Debug.Log($"Attack : {playerStatus.damageATK}");
                    break;

                case "YelloBerry":
                    Debug.Log("Eating Berrry");
                    Debug.Log($"HP = {playerStatus.HitPoint} DP ={playerStatus.DefendPoint}");
                    playerStatus.HitPoint += 5;
                    playerStatus.DefendPoint += 5;
                    Debug.Log($"HP = {playerStatus.HitPoint} DP ={playerStatus.DefendPoint}");
                    break;

                case "Defend":
                    Debug.Log("Eating Berrry");
                    playerStatus.recievedTempDef(0);
                    break;

                case "SpeedShoes":
                    Debug.Log("Eating Berrry");
                    playerStatus.recievedTempDef(1);
                    break;

                case "Spirit":
                    break;

                default:
                    Debug.LogWarning("Unknown PowerUp item: " + itemName);
                    break;
            }
        }
    }
}
