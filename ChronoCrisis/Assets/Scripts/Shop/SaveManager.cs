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
    
    
    private GameManager gameManager;
    private PlayerController playerController;
    public int level;
    public int HitPoint;
    public int Mana;
    public int DamageAttack;
    public int MagicPower;
    public int Defend;
    public int VitPoint;
    public int IntPoint;
    public int AgiPoint;
    public int Exp;
    public int MileStone;
    
    public bool[]itemUnlock = new bool[5] ;
    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (gameManager == null || playerController == null)
        {
            Debug.LogError("GameManager or PlayerController is NULL!");
        }
        
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
            itemUnlock = itemUnlock,
            level = level,
            HitPoint = HitPoint,
            Mana =  Mana,
            DamageAttack = DamageAttack,
            MagicPower = MagicPower,
            Defend = Defend,
            VitPoint = VitPoint,
            IntPoint = IntPoint,
            AgiPoint = AgiPoint,
            Exp = Exp,
            MileStone = MileStone   
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
        itemUnlock = data.itemUnlock ?? new bool[5];
        level = data.level;
        HitPoint = data.HitPoint;
        Mana =  data.Mana;
        DamageAttack = data.DamageAttack;
        MagicPower = data.MagicPower;
        Defend = data.Defend;
        VitPoint = data.VitPoint;
        IntPoint = data.IntPoint;
        AgiPoint = data.AgiPoint;
        Exp = data.Exp;
        MileStone = data.MileStone;        
    }

}

[SerializeField]
class PlayerData_Storage
{
    public int currentItem;
    public int money;
    public bool[] itemUnlock;
    public int level;
    public int HitPoint;
    public int Mana;
    public int DamageAttack;
    public int MagicPower;
    public int Defend;
    public int VitPoint;
    public int IntPoint;
    public int AgiPoint;
    public int Exp;
    public int MileStone;
}
