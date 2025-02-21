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
    private int worldLevel = 1;


    private bool isWorld1 = true;
    private bool isWorld2 = false;
    private bool isWorld3 = false;
    private bool isWorld4 = false;
    private bool isWorld5 = false;

    private PlayerController playerController;
    private EnemyController enemyController;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
        spawnManager = GetComponent<SpawnManager>();


        if(playerController == null || enemyController == null)
        {
            Debug.LogError("player or enemy is still NULL");
        }
        if(spawnManager == null)
        {
            Debug.LogError("player or enemy is still NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canLoop)
        {
            canLoop = false;
            loopTime++;
            spawnManager.ClearEnemies();

            CountUpRespawn();

            playerController.resetPositionPlayer();
            spawnManager.SpawnEnemis(enemyCount, loopTime, worldLevel);
            StartCoroutine(coolDownTimeLoop());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && playerController.isDead == true)
        {
            loopTime++;

            spawnManager.ClearEnemies();

            CountUpRespawn();

            playerController.resetPositionPlayer();
            spawnManager.SpawnEnemis(enemyCount, loopTime, worldLevel);
        }
    }

    public void ObjectiveToComplte()
    {
        if(playerController.enemyKilled == 69)
        {
            isChangeWorld = true;
        }
        else if(playerController.hasKey == true && isWorld2)
        {
            isChangeWorld = true;
        }
        else if (playerController.hasKey == true && isWorld3)
        {
            isChangeWorld = true;
        }
        else if (playerController.hasKey == true && isWorld4)
        {
            isChangeWorld = true;

        }
    }

    public void changeWorld()
    {
        if (isChangeWorld && isWorld1)
        {
            //load 2 world
            /*
             Load.sceen.2
            isWorld2 = true
            isChangeWorld = false
            isWorld1 = false
             */
        }
        if (isChangeWorld && isWorld2)
        {
            //load 3 world
            /*
             Load.sceen.3
            isWorld3 = true
            isChangeWorld = false
            isWorld2 = false
             */
        }
    }

    void CountUpRespawn()
    {
        if (isWorld1)
        {
            enemyCount = 50 + 15 * 1;
        }
        else if (isWorld2)
        {
            enemyCount = 50 + 15 * 2;

        }
        else if (isWorld3)
        {
            enemyCount = 50 + 15 * 3;
        }
        else if (isWorld4)
        {
            enemyCount = 50 + 15 * 4;
        }
        else if (isWorld5)
        {
            enemyCount = 50 + 15 * 5;
        }
    }


    IEnumerator coolDownTimeLoop()
    {
        yield return new WaitForSeconds(coolDownTime);
        canLoop = true;
    }
}
