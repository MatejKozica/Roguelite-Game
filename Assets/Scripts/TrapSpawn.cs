using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawn : MonoBehaviour
{
    public Transform[] trapSpawnPoints;
    public GameObject[] traps;
    public List<Vector2> loadedTraps;

    bool flag = false;

    void Update()
    {
        // Spawn traps in room
        if (Generation.firstStageDone && !flag)
        {
            int numberOfTraps = Random.Range(0, trapSpawnPoints.Length - 1);
            flag = true;

            for (int i = 0; i < numberOfTraps; i++)
            {
                int randomTrap = Random.Range(0, trapSpawnPoints.Length);
                Vector2 selectedPosition = new Vector2(trapSpawnPoints[randomTrap].position.x, trapSpawnPoints[randomTrap].position.y);

                if (!loadedTraps.Contains(selectedPosition))
                {
                    Instantiate(traps[0], selectedPosition, Quaternion.identity);
                    loadedTraps.Add(selectedPosition);
                }
            }
        }
    }
}
