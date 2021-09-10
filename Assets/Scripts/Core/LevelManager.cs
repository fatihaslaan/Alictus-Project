using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> levels = new List<GameObject>();

    static LevelManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            try
            {
                GlobalAttributes.LoadTheGame(); //Load datas at awake
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    void Update()
    {
        if (GameUI.gameStarted && !levels[GlobalAttributes.currentLevel].active)
        {
            levels[GlobalAttributes.currentLevel].SetActive(true);
        }
    }

    public static LevelManager GetInstance()
    {
        return instance;
    }
}
