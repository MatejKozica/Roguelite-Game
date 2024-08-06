using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{
    public GameObject[] tileSpawns;

    void Start(){
        int r = Random.Range(0, tileSpawns.Length);
        if(tileSpawns[r] != null) {
            GameObject gObject = Instantiate(tileSpawns[r], transform);
            Generation.tileDict.Add(gObject.transform.position, gObject);
        }
    }
}
