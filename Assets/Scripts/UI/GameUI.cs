using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static float levelPercentage;
    public static bool gameOver; //Restart
    public static bool gameStarted = false; //Deactivate start button

    float foodAnimationDuration = 4f;
    bool foodEarned = false;

    [SerializeField]
    Text percentageText; //Level progress text
    [SerializeField]
    Image levelFillImage; //Level progress bar
    [SerializeField]
    GameObject startButton,nextLevelButton, inventoryPanel;

    [SerializeField]
    List<Text> inventoyFoodsTexts; //To see how many foods we earned
    [SerializeField]
    List<GameObject> inventoryFoodLocations; //To locate and spawn foods to positions
    GameObject earnedFood;

    void Start()
    {
        if (!gameStarted)
        {
            startButton.SetActive(true);
            nextLevelButton.SetActive(false);
        }
        LoadInventory(); //Load inventory
    }

    void LoadInventory()
    {
        GameObject temp;
        for(int i=0;i<inventoryFoodLocations.Count;i++)
        {
            if(GlobalAttributes.earnedFoods[i]>0) //We earned that food
            {
                inventoyFoodsTexts[i].text=GlobalAttributes.earnedFoods[i]+"x"; //Amount
                temp= Instantiate(FoodManager.GetInstance().foods[i],inventoryFoodLocations[i].transform.position,Quaternion.identity); //Spawn all the foods that we earned to inventory panel
                temp.transform.SetParent(inventoryFoodLocations[i].transform); //In canvas of course
                temp.AddComponent<FoodAnimation>();
                Destroy(temp.GetComponent<Rigidbody>());
                temp.transform.localScale=new Vector3(300,300,300);
            }
        }
    }

    void Update()
    {
        percentageText.text = levelPercentage + "%";
        levelFillImage.fillAmount = levelPercentage / 100;
        if (levelPercentage == 100 && !foodEarned)
        {
            GlobalAttributes.NextLevel();
            foodEarned = true;
            earnedFood=Instantiate(FoodManager.GetInstance().foods[GlobalAttributes.lastEarnedFood],new Vector3(0,4,0),Quaternion.identity); //Instantiate food that we earned
            if(GlobalAttributes.lastEarnedFood==2)
                earnedFood.transform.position=new Vector3(0.12f,4,0);
            Destroy(earnedFood.GetComponent<Rigidbody>());
            earnedFood.AddComponent<FoodAnimation>();
            LoadInventory();
        }
        if (gameOver)
        {
            gameOver = false; //Failed level
            Restart();
        }
        if (foodEarned) //Lets show the food that we earned
        {
            foodAnimationDuration -= Time.deltaTime;
            if (foodAnimationDuration < 0) //Okay lets change the scene now
            {
                GlobalAttributes.currentLevel = (GlobalAttributes.currentLevel + 1) % LevelManager.GetInstance().levels.Count;
                GlobalAttributes.SaveTheGame();
                Restart();
            }
        }

    }

    public void NextLevel() //Cheat
    {
        levelPercentage = 100;
    }

    public void Restart()
    {
        levelPercentage = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        nextLevelButton.SetActive(true);
        gameStarted = true;
    }

    public void InventoryActivation(bool activate)
    {
        inventoryPanel.SetActive(activate);
    }
}