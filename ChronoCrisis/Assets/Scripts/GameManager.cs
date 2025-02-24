using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isChangeWorld = false;
    private bool isGameOver;
    private bool isGamePaused;
    private bool canLoop = true;
    private float coolDownTime = 30f;
    private int enemyCount = 10;
    public int loopTime = 0;
    public int worldLevel = 1; // Using an integer instead of multiple bools

    public PlayerController playerController; // Assign in Inspector
    public SpawnManager spawnManager; // Assign in Inspector
    private SceneController sceneController;
    public GameObject NPCLoop5;
    public GameObject GateWayBorder;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = GetComponent<SceneController>();
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>(); // Find dynamically
        }

        if (spawnManager == null)
        {
            spawnManager = FindObjectOfType<SpawnManager>(); // Find dynamically
        }

        if (playerController == null || spawnManager == null)
        {
            Debug.LogError("PlayerController or SpawnManager is missing!");
        }
        spawnManager.SpawnEnemies(enemyCount, loopTime, worldLevel);
        spawnManager.SpawnPowerUp();
        sceneController = FindObjectOfType<SceneController>(); // CARI di seluruh scene!
        if (sceneController == null)
        {
            Debug.LogError("SceneController not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ToUnlockNPCWorld2();
        if (Input.GetKeyDown(KeyCode.Space) && canLoop)
        {
            RestartLoop();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && playerController.isDead)
        {
            loopTime++;

            spawnManager.ClearEnemies();
            CountUpRespawn();

            playerController.resetPositionPlayer();
            spawnManager.SpawnEnemies(enemyCount, loopTime, worldLevel);

        



        }
    }
    public void ToUnlockNPCWorld2(){
        if(loopTime>=5){
            NPCLoop5.SetActive(true);
        }
    }

    void RestartLoop()
    {
        canLoop = false;
        loopTime++;

        spawnManager.ClearPowerUps();
        spawnManager.ClearEnemies();
        CountUpRespawn();

        playerController.resetPositionPlayer();
        spawnManager.SpawnEnemies(enemyCount, loopTime, worldLevel);
        spawnManager.SpawnPowerUp();

        StartCoroutine(CoolDownTimeLoop());
    }

    public void ObjectiveToComplete()
    {
        if (worldLevel == 1 && playerController.enemyKilled >= 69) 
    {
        // Move from World 1 to World 2
        Debug.Log("Moving from World 1 to World 2...");
        isChangeWorld = true;
        GateWayBorder.gameObject.SetActive(false);
        worldLevel = 2;
        ChangeWorld();
    }
    else if (worldLevel == 2 && playerController.hasKey == true)
    {
        // Move from World 2 to World 3
        Debug.Log("Moving from World 2 to World 3...");
        isChangeWorld = true;
        GateWayBorder.gameObject.SetActive(false);
        worldLevel = 3;
        ChangeWorld();
    }
    else if (worldLevel == 3 && playerController.hasKey == true)
    {
        // Move from World 3 to World 4
        Debug.Log("Moving from World 3 to World 4...");
        isChangeWorld = true;
        GateWayBorder.gameObject.SetActive(false);
        worldLevel = 4;
        ChangeWorld();
    }
    else
    {
        Debug.Log("Conditions not met yet.");
    }
    }

    public void ChangeWorld()
    {
        if (isChangeWorld)
        {
            sceneController.ChangeScene(worldLevel);

            // Load the next world scene (example)
            Debug.Log("Loading World " + worldLevel);
            // SceneManager.LoadScene(worldLevel);
        }
    }

    void CountUpRespawn()
    {
        enemyCount = 50 + (15 * worldLevel); // Dynamically scales with world level
    }

    IEnumerator CoolDownTimeLoop()
    {
        yield return new WaitForSeconds(coolDownTime);
        canLoop = true;
    }
}
