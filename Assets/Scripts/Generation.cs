using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public int length;
    public int height;
    public int scale;
    public GameObject[] startingRooms;
    public GameObject[] pathRooms; // 0:LR, 1:LRB, 2:LRT, 3:LRBT
    public GameObject[] fillerRooms;
    public GameObject[] endingRooms;
    public GameObject[] treasure;

    public static Dictionary<Vector2, GameObject> tileDict;
    public GameObject[,] roomArray;
    public List<Vector2> loadedRooms;
    public int delayTime;

    public static bool firstStageDone = false;
    public static bool secondStageDone = false;
    public static bool readyForPlayer = false;
    int direction; // 0 & 1 = right, 2 & 3 = left, 4 = down
    int delay = 0;

    public int seed;

    void Start(){
        firstStageDone = false;
        secondStageDone = false;
        readyForPlayer = false;
        roomArray = new GameObject[length, height];
        tileDict = new Dictionary<Vector2, GameObject>();
        // Random.InitState(seed);
        transform.position = new Vector2(Random.Range(0, length), 0);
        CreateRoom(startingRooms[0]);

        if(transform.position.x == 0)
            direction = 0;
        else if(transform.position.x == length - 1)
            direction = 2;
        else
            direction = Random.Range(0, 3);
    }

    void Update(){
        // Debug.Log("ready for player: " + readyForPlayer);
        // Debug.Log("second stage done: " + secondStageDone);

        if(!firstStageDone){
            if(delay >= delayTime) {
                delay = 0;
                // right
                if(direction == 0 || direction == 1){
                    if(transform.position.x < length - 1){
                        transform.position += Vector3.right;
                        int r = 0;
                        CreateRoom(pathRooms[r]);
                        direction = Random.Range(0, 5);
                        if(direction == 2)
                            direction = 1;
                        else if(direction == 3)
                            direction = 4;
                    } else {
                        direction = 4;
                    }
                }

                // left
                else if(direction == 2 || direction == 3){
                    if(transform.position.x > 0){
                        transform.position += Vector3.left;
                        int r = 0;
                        CreateRoom(pathRooms[r]);
                        direction = Random.Range(0, 5);
                        if(direction == 0)
                            direction = 2;
                        else if(direction == 1)
                            direction = 4;
                    } else {
                        direction = 4;
                    }
                }

                // down
                else if(direction == 4){
                    if(transform.position.y > -height + 1){
                        Destroy(GetRoom(transform.position));
                        int r = (Random.Range(0,2) == 0) ? 1 : 3;
                        CreateRoom(pathRooms[r]);
                        transform.position += Vector3.down;
                        int rand = Random.Range(0, 4);
                        if(rand == 0){
                            if(transform.position.y > -height + 1){
                                CreateRoom(pathRooms[3]);
                                transform.position += Vector3.down;
                            } else {
                                direction = 4;
                                return;
                            }
                        }
                        int r1 = Random.Range(2, 4);
                        CreateRoom(pathRooms[r1]);
                        if(transform.position.x == 0)
                            direction = 0;
                        else if(transform.position.x == length - 1)
                            direction = 2;
                        else
                            direction = Random.Range(0, 4);
                    }
                    // If a room cannot be generated create exit
                    else {
                        Destroy(GetRoom(transform.position));
                        CreateRoom(endingRooms[0]);
                        FillMap();
                        firstStageDone = true;
                        delay = 1;
                    }
                } 
            }      
            else {
                delay++;
            }
            
        } 
        
        else {
            if(!secondStageDone && delay >= 1) {
                // Treasure generation
                for(int x = 0; x < length * scale; x++) {
                    for(int y = 0; y  > -height * scale; y--) { 
                        Vector2 pos = new Vector2(x, y);
                        if(!tileDict.ContainsKey(pos) && HasFloor(pos) ) {
                            int count = GetAdjacentCount(pos);

                            if(count > 3 && Random.Range(0,4) == 0) {
                                int index = Random.Range(0, treasure.Length);
                                Debug.Log("Index: " + index);
                                Instantiate(treasure[index], pos, Quaternion.identity);
                            }
                        }
                    }
                }

                secondStageDone = true;
                readyForPlayer = true;
            } else if(secondStageDone) {
                delay++;
            }
        }
    }

    int GetAdjacentCount(Vector2 pos) {
        int counter = 0;
        for(int x = -1; x < 2; x++) {
            for(int y = -1; y < 2; y++) {
                Vector2 newPos = pos + new Vector2(x, y);
                if(newPos == pos) {
                    continue;
                }

                if(tileDict.ContainsKey(newPos)) {
                    counter++;
                }
            }
        }

        return counter;
    } 

    bool HasFloor(Vector2 pos) {
        return tileDict.ContainsKey(pos + Vector2.down);
    }

    // Generate rest of the rooms
    void FillMap(){
        for(int y = 0; y < height; y++){
            for(int x = 0; x < length; x++){
                if(!loadedRooms.Contains(new Vector2(x, y))){
                    int r = Random.Range(0, fillerRooms.Length);
                    transform.position = new Vector2(x, -y);
                    CreateRoom(fillerRooms[r]);
                }
            }
        }
    }

    void CreateRoom(GameObject room){
        GameObject temp = Instantiate(room, transform.position * scale, Quaternion.identity);
        int x = (int)transform.position.x;
        int y = -(int)transform.position.y;
        roomArray[x, y] = temp;
        loadedRooms.Add(new Vector2(x,y));
    }

    GameObject GetRoom(Vector2 position){
        return roomArray[(int)position.x, -(int)position.y];
    }

    public static void NextLevel() {
        PlayerData.levelNumber++;
    }
}
