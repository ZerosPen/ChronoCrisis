using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private Button use;
    [SerializeField] private Button buy;
    [SerializeField] private Text priceText;


    [SerializeField]private int[] itemPrices;
    private int currentItem;

    private void Start()
    {
        currentItem = SaveManager.instance.currentItem;
        SelectItem(currentItem);
    }

    private void SelectItem(int _index)
    {
        for(int i=0; i<transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);

        UpdateUI();

    }

    private void UpdateUI()
    {
    
        if (SaveManager.instance.itemUnlock[currentItem])
        {
            use.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);

        }
        else
        {
            use.gameObject.SetActive(false);
            buy.gameObject.SetActive(true); 
            priceText.text = itemPrices[currentItem] + "$"; 

            buy.interactable = (SaveManager.instance.money >= itemPrices[currentItem]);

        }
    }

    private void Update()
    {
        if(buy.gameObject.activeInHierarchy)
        {
            buy.interactable = (SaveManager.instance.money >= itemPrices[currentItem]);
        }
    }

    public void ChangeItem(int _change)
    {
        currentItem += _change;

        if (currentItem > transform.childCount-1)
            currentItem = 0;
        else if(currentItem < 0)
            currentItem = transform.childCount-1;
        
        SaveManager.instance.currentItem = currentItem;
        SaveManager.instance.Save();

        SelectItem(currentItem);
    }
    public void buyItems()
    {
        SaveManager.instance.money -=itemPrices[currentItem];
        SaveManager.instance.itemUnlock[currentItem] = true;
        SaveManager.instance.Save();
        UpdateUI();
    }
}
