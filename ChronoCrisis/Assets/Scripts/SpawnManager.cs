using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerUpPrefab;
    public Transform[] powerUpSpawn;
    public Transform[] spawnPoint;

    private List<GameObject> enemies = new List<GameObject>();

    public void SpawnEnemis(int count, int timeLoop, int worldLevel)
    {
        for (int i = 0; i < count; i++)
        {
            Transform randomSpawnPoint = spawnPoint[Random.Range(0, spawnPoint.Length)];
            GameObject randomEnemyPrefab = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
            GameObject newEnemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.ScalingUp(timeLoop, worldLevel);
            }

            enemies.Add(newEnemy);
        }
    }

    public void ClearEnemies()
    {
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
    }
        
}
