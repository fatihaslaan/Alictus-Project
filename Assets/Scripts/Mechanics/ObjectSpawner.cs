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
    bool objectsCanCollide; //Can objects collide with each other?

    Bounds spawningAreaBounds; //Getting bounds of area
    Vector3 spawningAreaCenter;
    List<GameObject> spawnedObjects = new List<GameObject>();

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
        if (objectsCanCollide)
        {
            FoodMatcher.totalMatches=0; //Reset
            FoodMatcher.possibleMatches=0;
        }
        for (int i = 0; i < spawnQuantity; i++)
        {
            do
            {
                x = Random.Range(spawningAreaCenter.x - spawningAreaBounds.extents.x, spawningAreaCenter.x + spawningAreaBounds.extents.x); //Spawn objects inside bounds
                z = Random.Range(spawningAreaCenter.z - spawningAreaBounds.extents.z, spawningAreaCenter.z + spawningAreaBounds.extents.z);
            } while (CloseToSpawnedObject(x, z)); //They should be far away from each other
            int randomFood = Random.Range(0, spawningObjectTypes.Count); //Spawn random food
            spawnedObjects.Add(Instantiate(spawningObjectTypes[randomFood], new Vector3(x, spawningObjectTypes[randomFood].transform.position.y, z), Quaternion.identity));
            if (objectsCanCollide)
            {
                spawnedObjects[spawnedObjects.Count - 1].AddComponent<FoodMatcher>(); //If they collide they should match
                FoodMatcher.possibleMatches++; //To calculate percentage
            }
        }
    }

    bool CloseToSpawnedObject(float x, float z)
    {
        if (!objectsCanCollide)
            return false;
        foreach (GameObject food in spawnedObjects)
        {
            if (Vector3.Distance(new Vector3(x, 0, z), food.transform.position) < 0.2f)
                return true; //They are close to each other
        }
        return false;
    }
}