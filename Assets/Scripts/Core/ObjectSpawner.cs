using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawningArea; //Objects will spawn in this area
    [SerializeField]
    List<GameObject> spawningObjectTypes; //Which object types we want to spawn
    [SerializeField]
    int spawnQuantity; //Number of spawning
    [SerializeField]
    bool objectColliding; //Can objects collide with each other?

    Bounds spawningAreaBounds; //Getting bounds of area
    Vector3 spawningAreaCenter;

    void Awake()
    {
        spawningAreaBounds = spawningArea.GetComponent<Collider>().bounds;
        spawningAreaCenter = spawningAreaBounds.center;
        Spawn();
    }

    void Spawn()
    {
        float x = 0;
        float z = 0;
        for (int i = 0; i < spawnQuantity; i++)
        {
            x = Random.Range(spawningAreaCenter.x - spawningAreaBounds.extents.x, spawningAreaCenter.x + spawningAreaBounds.extents.x); //Spawn objects inside bounds
            z = Random.Range(spawningAreaCenter.z - spawningAreaBounds.extents.z, spawningAreaCenter.z + spawningAreaBounds.extents.z);
            int randomFood=Random.Range(0,spawningObjectTypes.Count); //Spawn random food
            Instantiate(spawningObjectTypes[randomFood],new Vector3(x,spawningObjectTypes[randomFood].transform.position.y,z),Quaternion.identity);
        }
    }
}