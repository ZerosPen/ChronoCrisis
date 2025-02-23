using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerUpPrefab;
    public Transform[] powerUpSpawn;
    public Transform[] spawnPoint;

    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> powerUps = new List<GameObject>();

    public void SpawnEnemies(int count, int timeLoop, int worldLevel)
    {
        if (enemyPrefab.Length == 0 || spawnPoint.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs or spawn points assigned!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Transform randomSpawnPoint = spawnPoint[Random.Range(0, spawnPoint.Length)];
            GameObject randomEnemyPrefab = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
            GameObject newEnemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            if (newEnemy.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.ScalingUp(timeLoop, worldLevel);
            }

            enemies.Add(newEnemy);
        }
    }

    public void SpawnPowerUp()
    {
        Debug.Log("SpawnPowerUp method called!"); // Debugging line

        if (powerUpPrefab.Length == 0 || powerUpSpawn.Length == 0)
        {
            Debug.LogWarning("No power-up prefabs or spawn points assigned!");
            return;
        }

        Transform randomSpawnPoint = powerUpSpawn[Random.Range(0, powerUpSpawn.Length)];
        GameObject randomPowerUp = powerUpPrefab[Random.Range(0, powerUpPrefab.Length)];
        GameObject newPowerUp = Instantiate(randomPowerUp, randomSpawnPoint.position, Quaternion.identity);
        powerUps.Add(newPowerUp);
    }

    public void ClearEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
        enemies.Clear();
    }

    public void ClearPowerUps()
    {
        for (int i = powerUps.Count - 1; i >= 0; i--)
        {
            if (powerUps[i] != null)
            {
                Destroy(powerUps[i]);
            }
        }
        powerUps.Clear();
    }
}
