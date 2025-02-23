using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyAdd : MonoBehaviour
{
    public GameObject BuyFireball;
    public void BuyFireballPanel(){
        BuyFireball.SetActive(false);
        SaveManager.instance.money -= 7;
    }
    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SaveManager.instance.money -= 100;
            SaveManager.instance.Save();
        }
            
    }

    public void AddMoney(int coin)
    {
        SaveManager.instance.money += coin;
        SaveManager.instance.Save();
    }
}
