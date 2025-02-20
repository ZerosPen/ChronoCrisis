using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Items : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private float restoreValue;

    public string ItemName => itemName;
    public float RestoreValue => restoreValue;

    public virtual void UseItem() { }
}
