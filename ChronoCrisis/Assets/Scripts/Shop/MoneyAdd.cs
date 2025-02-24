using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyAdd : MonoBehaviour
{
    public GameObject BuyFireball;
    public void BuyFireballPanel(){
        SaveManager.instance.money -= 8;
    }

    public GameObject BuyWaterJet;
    public void BuyWaterJetPanel(){
        SaveManager.instance.money -= 8;
    }
    public GameObject AirShot;
    public void BuyAirShot(){
        SaveManager.instance.money -= 8;
    }
    public GameObject FireWall;
    public void FireWallPanel(){
        SaveManager.instance.money -= 14;
    }
    public GameObject LightningBolt;
    public void LightningBoltPanel(){
        SaveManager.instance.money -= 14;
    }
    public GameObject IcycleShot;
    public void IcycleShotPanel(){
        SaveManager.instance.money -= 14;
    }
    public GameObject WindCutter;
    public void WindCutterPanel(){
        SaveManager.instance.money -= 14;
    }
    public GameObject FireJavalin;
    public void FireJavalinPanel(){
        SaveManager.instance.money -= 30;
    }
    public GameObject TyphoonGold;
    public void TyphoonGoldPanel(){
        SaveManager.instance.money -= 30;
    }
    public GameObject DialogWorld2;
    public void DialogWorld2Shop(){
        SaveManager.instance.money -= 100;
    }



    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SaveManager.instance.money += 100;
            SaveManager.instance.Save();
        }
            
    }

    public void AddMoney(int coin)
    {
        SaveManager.instance.money += coin;
        SaveManager.instance.Save();
    }
}
