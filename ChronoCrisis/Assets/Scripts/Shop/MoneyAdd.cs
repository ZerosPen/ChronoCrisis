using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyAdd : MonoBehaviour
{

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveManager.instance.money += 100;
            SaveManager.instance.Save();
        }
            
        else if(Input.GetKeyDown(KeyCode.F))
        {
            SaveManager.instance.money -= 100;
            SaveManager.instance.Save();
        }
            
    }
}
