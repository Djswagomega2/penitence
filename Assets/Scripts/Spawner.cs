using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Set up SpawnerID
    //Remove the enemy from the list of the spawner where the come from when they die
    public GameObject[] enemies;
    public List<GameObject> spawnedEnemies;
    public float enemySpawnCoolDown;
    public float enemySpawnCoolTime;
    public int enemyCount;
    public int maxEnemies;
    public bool isSpawningEnemy;

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (isSpawningEnemy)
        {
            GameObject spawnedEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
            //spawnedEnemy.GetComponent<Enemy>().spanwerID = this.gameObject;
            spawnedEnemies.Add(spawnedEnemy);
            enemyCount++;
            isSpawningEnemy = false;

        }

        if (!isSpawningEnemy)
        {
            enemySpawnCoolDown -= Time.deltaTime;

            if (enemySpawnCoolDown < 0)
            {
                enemySpawnCoolDown = enemySpawnCoolTime;
                isSpawningEnemy = true;
            }
        }

        if(spawnedEnemies.Count <= 0)
        {
            enemyCount = 0;
        }

        //if we need to have a kill switch down the line
        if (!stopSpawning())
       {
            if (!isSpawningEnemy)
            {
                enemySpawnCoolDown -= Time.deltaTime;

                if (enemySpawnCoolDown < 0)
                {
                    enemySpawnCoolDown = enemySpawnCoolTime;
                    isSpawningEnemy = true;
                }
            }
       }
        else
        {
            isSpawningEnemy = false;
            enemySpawnCoolDown = 0;
        }

    }

    public bool stopSpawning()
    {
        if (enemyCount >= maxEnemies)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
