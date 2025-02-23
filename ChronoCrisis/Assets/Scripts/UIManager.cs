using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ButtonManagement bm;

    [SerializeField] private Text hpMax;
    [SerializeField] private Text hpCurr;
    [SerializeField] private Text manaMax;
    [SerializeField] private Text manaCurr;

    private PlayerController playerController;
    private GameManager gameManager;

    [Header("StatusProfile")]
    [SerializeField] private Text LevelValue;
    [SerializeField] private Text HitpointValue;
    [SerializeField] private Text ManaValue;
    [SerializeField] private Text DamageAttackValue;
    [SerializeField] private Text MagicPowerValue;
    [SerializeField] private Text Defend;
    [SerializeField] private Text VitPoint;
    [SerializeField] private Text IntPoint;
    [SerializeField] private Text AgiPoint;


    private void Awake()
    {
        // Find the GameManager & PlayerController correctly
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (gameManager == null || playerController == null)
        {
            Debug.LogError("GameManager or PlayerController is NULL!");
        }

        if (hpMax == null || hpCurr == null || manaMax == null || manaCurr == null)
        {
            Debug.LogError("UI Text components are not assigned! Assign them in the Inspector.");
        }
    }

    public void UpdateHealth(float healthNow, float healthMax)
    {
        if (hpCurr != null) hpCurr.text = healthNow.ToString();
        if (hpMax != null) hpMax.text = "/" + healthMax.ToString();
    }

    public void UpdateMana(float manaNow, float ManaMax)
    {
        if (hpCurr != null) manaCurr.text = manaNow.ToString();
        if (hpMax != null) manaMax.text = "/" + ManaMax.ToString();
    }

    public void UpdateStatus(int level,float Hitpoint, float mana, float damage, float magicPower, int defend, int VIT, int AGI, int INT)
    {
        if (LevelValue != null && HitpointValue != null && ManaValue != null
            && DamageAttackValue != null && MagicPowerValue != null &&
            Defend != null && VitPoint != null && AgiPoint != null && IntPoint != null
            )
        {
            LevelValue.text = level.ToString();
            HitpointValue.text = Hitpoint.ToString();
            ManaValue.text = mana.ToString();
            DamageAttackValue.text = damage.ToString();
            MagicPowerValue.text = magicPower.ToString();
            Defend.text = defend.ToString();
            VitPoint.text = VIT.ToString();
            AgiPoint.text = AGI.ToString();
            IntPoint.text = INT.ToString();
        }
    }

}
