using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagement : MonoBehaviour
{

    //shop
    public GameObject SkillShopPanel;
    public GameObject WeaponShopPanel;
    public GameObject PotionShopPanel;
    public GameObject ShopPanel;
    

    public void ClickToOpenSkillShopPanel(){
        SkillShopPanel.SetActive(true);
        WeaponShopPanel.SetActive(false);
        PotionShopPanel.SetActive(false);
    }
    public void ClickToCloseShop(){
        ShopPanel.SetActive(false);
    }
    public void ClickToOpenWeaponShopPanel(){
        SkillShopPanel.SetActive(false);
        WeaponShopPanel.SetActive(true);
        PotionShopPanel.SetActive(false);
    }

    public void ClickToOpenPotionShopPanel(){
        SkillShopPanel.SetActive(false);
        WeaponShopPanel.SetActive(false);
        PotionShopPanel.SetActive(true);
    }


    //Main page
    public GameObject Setting;
    public GameObject About;
    public GameObject Exit;
    public void ClickToOpenSetting(){
        Setting.SetActive(true);
    }
    public void ClickToCloseSetting(){
        Setting.SetActive(false);
    }
    public void ClickToOpenAbout(){
        About.SetActive(true);
    }
    public void ClickToCloseAbout(){
        About.SetActive(false);
    }
    public void ClickToOpenExit(){
        Exit.SetActive(true);
    }
    public void ClickToCloseExit(){
        Exit.SetActive(false);
    }
        public void ClickToExit(){
        Application.Quit();
    }
    public GameObject PausePanel;

    public void ClickToOpenPause(){
        PausePanel.SetActive(true);
    }
    public void ClickToClosePause(){
        PausePanel.SetActive(false);
    }

}
