using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] ButtonManagement bm;
    [SerializeField] private Text hpMax;
    [SerializeField] private Text hpCurr;

    [SerializeField] private Text manaMax;
    [SerializeField] private Text manaCurr;
    
    private PlayerController playerController;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        playerController = GetComponent<PlayerController>();
       
         if ( gameManager == null || playerController == null)
        {
            Debug.LogError("GameManager & PlayerController is NULL");
        }
        hpMax = GetComponent<Text>();
        hpCurr = GetComponent<Text>();
        manaMax = GetComponent<Text>();
        manaCurr = GetComponent<Text>();
        
    }

    public void UpdateHealth(float healthNow, float healthMax)
    {
        hpCurr.text = healthNow.ToString();
        hpMax.text = "/" + healthMax.ToString();
    }




}


