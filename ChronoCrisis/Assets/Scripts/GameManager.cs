using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isChangeWorld;
    private bool isGameOver;
    private bool isGamePaused;
    private bool canLoop = true;
    private float coolDownTime = 30f;
    private int enemyCount = 10;
    public int loopTime = 0;
    private int worldLevel = 1; // Using an integer instead of multiple bools

    public PlayerController playerController; // Assign in Inspector
    public SpawnManager spawnManager; // Assign in Inspector

    // Start is called before the first frame update
    void Start()
    {
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
        spawnManager.SpawnEnemis(enemyCount, loopTime, worldLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canLoop)
        {
            RestartLoop();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && playerController.isDead)
        {
            RestartLoop();
        }
    }

    void RestartLoop()
    {
        canLoop = false;
        loopTime++;

        spawnManager.ClearEnemies();
        CountUpRespawn();

        playerController.resetPositionPlayer();
        spawnManager.SpawnEnemis(enemyCount, loopTime, worldLevel);

        StartCoroutine(CoolDownTimeLoop());
    }

    public void ObjectiveToComplete()
    {
        if (playerController.enemyKilled >= 69 && worldLevel == 1)
        {
            isChangeWorld = true;
        }
        else if (playerController.hasKey && worldLevel > 1)
        {
            isChangeWorld = true;
        }
    }

    public void ChangeWorld()
    {
        if (isChangeWorld)
        {
            worldLevel++; // Move to the next world
            isChangeWorld = false;

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
