using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : MonoBehaviour
{
    public GameObject player;
    public bool hasPlayerSpawned = false;
    public Transform spawnPosition;

    void Update()
    {
        if(Generation.readyForPlayer && !hasPlayerSpawned){
            // Spawn in player game object.
            Instantiate(player, spawnPosition);
            hasPlayerSpawned = true;
        }
    }
}
