using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawn : MonoBehaviour
{
    public Transform[] trapSpawnPoints;
    public Transform[] enemiesSpawnPoints;
    public GameObject[] traps;
    public GameObject[] enemies;
    public List<Vector2> loadedEntities;

    bool flag = false;

    void Update()
    {

        if (Generation.firstStageDone && !flag)
        {

            flag = true;

            Vector2 selectedPosition;

            // Spawn enemies
            int numberOfEnemies = Random.Range(1, enemiesSpawnPoints.Length);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int randomEnemySpawnPoint = Random.Range(0, enemiesSpawnPoints.Length);
                selectedPosition = new Vector2(trapSpawnPoints[randomEnemySpawnPoint].position.x, trapSpawnPoints[randomEnemySpawnPoint].position.y);
                if (!loadedEntities.Contains(selectedPosition))
                {
                    Instantiate(enemies[0], selectedPosition, Quaternion.identity);
                    loadedEntities.Add(selectedPosition);
                }
            }

            // Spawn traps in room
            int numberOfTraps = Random.Range(0, trapSpawnPoints.Length - 1);
            for (int i = 0; i < numberOfTraps; i++)
            {
                int randomTrapSpawnPoint = Random.Range(0, trapSpawnPoints.Length);
                selectedPosition = new Vector2(trapSpawnPoints[randomTrapSpawnPoint].position.x, trapSpawnPoints[randomTrapSpawnPoint].position.y);

                if (!loadedEntities.Contains(selectedPosition))
                {
                    Instantiate(traps[0], selectedPosition, Quaternion.identity);
                    loadedEntities.Add(selectedPosition);
                }
            }
        }
    }
}
