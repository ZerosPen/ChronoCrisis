using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class InventoryItems : MonoBehaviour
{
     public Items items;
    
    public Image image;
    public TMP_Text countText;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int count =1;

    private void Awake()
    {
        // Ensure the image component is assigned
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }
    public void InitialiseItem(Items newItems2){
        items = newItems2;
        image.sprite = newItems2.image;
        RefreshCount();
    }
    public void RefreshCount(){
        countText.text = count.ToString();
        bool textActive = count>1;
        countText.gameObject.SetActive(textActive);
    }

}
