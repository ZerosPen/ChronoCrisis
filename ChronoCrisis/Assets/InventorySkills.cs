
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class InventorySkills : MonoBehaviour
{
    [HideInInspector] public AoE items;
    
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
    public void InitialiseItem(AoE newItems){
        items = newItems;
        image.sprite = newItems.image;
        RefreshCount();
    }
    public void RefreshCount(){
        countText.text = count.ToString();
        bool textActive = count>1;
        countText.gameObject.SetActive(textActive);
    }

}
