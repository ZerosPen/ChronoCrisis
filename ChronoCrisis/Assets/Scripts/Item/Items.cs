using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(menuName = "Items")]
public abstract class Items : ScriptableObject
{

    public Sprite image;
   // public Vector2Int range = new Vector2Int(5,4);

    public bool stackable = true;

    public string itemName;
    public string itemType;
    public float valueItem;
    public float priceItem;

    public virtual void UseItem(GameObject Player) { }
}
