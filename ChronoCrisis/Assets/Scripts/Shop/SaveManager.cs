using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

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
    public int worldLevel;

    public bool[] itemUnlock = new bool[5];

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
    }

    private void Start()
    {
        Load();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerAndGameManager(); // Find references again
        Load(); // Reload data
    }

    private void FindPlayerAndGameManager()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found after scene load!");
        }
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/playerInfo.json";

        if (gameManager != null)
        {
            worldLevel = gameManager.worldLevel; // Save world level
        }

        PlayerData_Storage data = new PlayerData_Storage
        {
            currentItem = currentItem,
            money = money,
            itemUnlock = itemUnlock,
            level = level,
            HitPoint = HitPoint,
            Mana = Mana,
            DamageAttack = DamageAttack,
            MagicPower = MagicPower,
            Defend = Defend,
            VitPoint = VitPoint,
            IntPoint = IntPoint,
            AgiPoint = AgiPoint,
            Exp = Exp,
            MileStone = MileStone,
            worldLevel = worldLevel // Save world level
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
        Mana = data.Mana;
        DamageAttack = data.DamageAttack;
        MagicPower = data.MagicPower;
        Defend = data.Defend;
        VitPoint = data.VitPoint;
        IntPoint = data.IntPoint;
        AgiPoint = data.AgiPoint;
        Exp = data.Exp;
        MileStone = data.MileStone;

        // Apply loaded data to player
        if (playerController != null)
        {
            playerController.level = level;
            playerController.HitPoint = HitPoint;
            playerController.manaPoint = Mana;
            playerController.damageATK = DamageAttack;
            playerController.magicPower = MagicPower;
            playerController.DefendPoint = Defend;
            playerController.pointVit = VitPoint;
            playerController.pointInt = IntPoint;
            playerController.pointAgi = AgiPoint;
            playerController.playerExp = Exp;
            playerController.nextMilestone = MileStone;
            playerController.coins = money;
        }
    }

    public void UpdatePlayerData()
    {
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController is NULL. Skipping data update.");
            return;
        }

        level = playerController.level;
        HitPoint = (int)playerController.HitPoint;
        Mana = (int)playerController.manaPoint;
        DamageAttack = (int)playerController.damageATK;
        MagicPower = (int)playerController.magicPower;
        Defend = playerController.DefendPoint;
        VitPoint = playerController.pointVit;
        IntPoint = playerController.pointInt;
        AgiPoint = playerController.pointAgi;
        Exp = playerController.playerExp;
        MileStone = playerController.nextMilestone;
        money = playerController.coins;
    }
}


[System.Serializable]
public class PlayerData_Storage
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
    public int worldLevel; // Add world level
}

