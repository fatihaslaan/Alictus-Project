using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public List<GameObject> foods; //To see all food objects

    static FoodManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static FoodManager GetInstance()
    {
        return instance;
    }
}