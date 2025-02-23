using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagement : MonoBehaviour
{

    //shop
    public GameObject SkillShopPanel;
    public GameObject itemShopPanel;
    public GameObject WeaponShopPanel;
    public GameObject PotionShopPanel;
    public GameObject ShopPanel;
    public GameObject StatusPlayer;
    public GameObject PauseButton;
    

    public void ClickToOpenShopPanel(){
        ShopPanel.SetActive(true);
        StatusPlayer.SetActive(false);
        PauseButton.SetActive(false);
    }

    public void ClickOpenitemlShop()
    {
        itemShopPanel.SetActive(true);
        SkillShopPanel.SetActive(false);
    }

    public void ClickOpenSkillShop()
    {
        SkillShopPanel.SetActive(true);
        itemShopPanel.SetActive(false);
    }

    public void ClickToCloseShopPanel()
    {
        if (ShopPanel == null)
        {
            Debug.LogWarning("ShopPanel is not assigned in the Inspector!");
            return;
        }

        ShopPanel.SetActive(false);
        StatusPlayer.SetActive(true);
        PauseButton.SetActive(true);

        Debug.Log("ShopPanel closed successfully.");
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

    //MainGame
    public GameObject PanelProfile;
    public void ClickOpenToProfile()
    {
        PanelProfile.SetActive(true);
    }
    public void ClickCloseToProfile()
    {
        PanelProfile.SetActive(false);
    }
}
