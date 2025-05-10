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
    public float radius;
    [SerializeField] float minPositionShift;
    [SerializeField] float maxPositionShift;

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        float angleStep = 360 / maxEnemies;
        float startAngle = 0;
        if (isSpawningEnemy)
        {
            //float angle = startAngle + (enemyCount * angleStep);
            float angle = startAngle + (enemyCount % maxEnemies) * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject spawnedEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)],
                                      transform.position + (Vector3)(direction * Random.Range(minPositionShift, maxPositionShift)),
                                      Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().spawner = gameObject;
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
