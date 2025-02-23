using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance {get; private set;}

    public int currentItem;
    public int money;
    public bool[]itemUnlock = new bool[46] {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        
        DontDestroyOnLoad(gameObject);
        Load();
    }
    public void Save()
    {
        string path = Application.persistentDataPath + "/playerInfo.json";
        PlayerData_Storage data = new PlayerData_Storage
        {
            currentItem = currentItem,
            money = money,
            itemUnlock = itemUnlock
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/playerInfo.json";

        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found.");
            return;
        }

        string json = File.ReadAllText(path);
        PlayerData_Storage data = JsonUtility.FromJson<PlayerData_Storage>(json);

        currentItem = data.currentItem;
        money = data.money;
        itemUnlock = data.itemUnlock ?? new bool[46];
    }

}

[SerializeField]
class PlayerData_Storage
{
    public int currentItem;
    public int money;
    public bool[] itemUnlock;
}
