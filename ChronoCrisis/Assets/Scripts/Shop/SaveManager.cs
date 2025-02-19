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
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);

            currentItem = data.currentItem;
            money = data.money;
            itemUnlock = data.itemUnlock;

            if(data.itemUnlock==null){
                itemUnlock = new bool[46];
                
            }

            file.Close();
        }
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage();

        data.currentItem = currentItem;
        data.money = money;
        data.itemUnlock = itemUnlock;

        bf.Serialize(file, data);
        file.Close();
    }

}

[SerializeField]
class PlayerData_Storage
{
    public int currentItem;
    public int money;
    public bool[] itemUnlock;
}
