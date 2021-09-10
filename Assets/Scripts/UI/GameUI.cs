using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static float levelPercentage;
    public static bool gameOver;

    [SerializeField]
    Text percentageText;

    void Update()
    {
        percentageText.text=levelPercentage+"%";
        if(levelPercentage==100)
        {
            GlobalAttributes.NextLevel();
            Restart();
        }
        if(gameOver)
        {
            gameOver=false;
            Restart();
        }
    }

    public void Restart()
    {
        levelPercentage=0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}