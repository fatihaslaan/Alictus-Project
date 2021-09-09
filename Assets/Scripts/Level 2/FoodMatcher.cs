using UnityEngine;

public class FoodMatcher : MonoBehaviour
{
    public static int possibleMatches; //Calculating percentage
    static int totalMatches=0;

    static bool matched=false;
    GameObject newFood;
    int id;

    void Start()
    {
        matched=false;
        for(int i=0;i<FoodManager.GetInstance().foods.Count;i++)
        {
            if(FoodManager.GetInstance().foods[i].tag==tag)
            {
                id=i; //To upgrade food
            }
        }
        if(id==FoodManager.GetInstance().foods.Count-1)
        {
            //GameOver
        }
    }

    void OnCollisionEnter(Collision col) 
    {
        if(col.transform.tag==tag&&!matched)
        {
            matched=true; //To prevent calling this function twice
            Destroy(col.gameObject); //Destroy other food

            newFood=Instantiate(FoodManager.GetInstance().foods[id+1],transform.position,Quaternion.identity); //Instantiate new food
            newFood.AddComponent<FoodMatcher>();

            totalMatches++; //We have progressed
            GameUI.levelPercentage=(float)totalMatches*100/possibleMatches; //Refresh percentage

            Destroy(gameObject); //Destroy this object
        }
    }
}