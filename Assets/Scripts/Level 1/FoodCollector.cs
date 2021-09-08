using System.Collections.Generic;
using UnityEngine;

public class FoodCollector : MonoBehaviour
{
    [SerializeField]
    List<string> objectTagsToCollect; //Which foods we will collect

    List<GameObject> objectsToCollect = new List<GameObject>(); //Objects that we are going to collect
    List<Rigidbody> objectRigidbodies=new List<Rigidbody>(); //Rigidbodies of these objects to stop them
    List<MeshRenderer> objectmaterials=new List<MeshRenderer>(); //MeshRenderers of these objects to change their material 
    Material collectedFoodMaterial; //We will use FoddCollector's material

    float distanceToCollect=0.2f; //Collect range
    int collectableObjectCount; //To calculate percentage

    void Start()
    {
        foreach (string tag in objectTagsToCollect) //Adding objects and their assets, that we are going to collect 
        {
            foreach (GameObject food in GameObject.FindGameObjectsWithTag(tag))
            {
                objectsToCollect.Add(food);
                objectRigidbodies.Add(food.GetComponent<Rigidbody>());
                objectmaterials.Add(food.GetComponent<MeshRenderer>());
            }
        }
        collectedFoodMaterial=GetComponent<MeshRenderer>().material;
        collectableObjectCount=objectsToCollect.Count;
    }

    void Update()
    {
        for(int i=0;i<objectsToCollect.Count;i++)
        {
            if(Vector3.Distance(transform.position,objectsToCollect[i].transform.position)<distanceToCollect)
            {
                objectRigidbodies[i].isKinematic=true; //Stop collected olive
                objectmaterials[i].material=collectedFoodMaterial; //Change material
                distanceToCollect+=0.01f; //Increase range
                objectsToCollect.RemoveAt(i); //Remove from list so we dont check them again
                objectRigidbodies.RemoveAt(i);
                objectmaterials.RemoveAt(i);
                GameUI.levelPercentage=(float)(collectableObjectCount-objectsToCollect.Count)*100/collectableObjectCount; //Calculate percentage
                break;
            }
        }
    }
}