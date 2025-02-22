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
}
