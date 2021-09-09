using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> levels=new List<GameObject>();

    static LevelManager instance;
   
    void Awake() 
    {
        if(instance!=null&&instance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance=this;
            levels[GlobalAttributes.currentLevel].SetActive(true);
        }
    }

    public static LevelManager GetInstance()
    {
        return instance;
    }
}
