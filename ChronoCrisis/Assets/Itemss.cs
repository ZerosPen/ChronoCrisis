using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Scriprable object/itemss")]
public class Itemss : ScriptableObject
{
    public TileBase title;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);


    public bool stackable = true;

    public Sprite image;
}

public enum ItemType{
    potion,
    weapon,
    physical,
    magic,
    skill,

}
public enum ActionType{
    heal,
}

