using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static float levelPercentage;

    [SerializeField]
    Text percentageText;

    void Update()
    {
        percentageText.text=levelPercentage+"%";
        if(levelPercentage==100)
        {
            //Level Complete
        }
    }

    public void Restart()
    {
        levelPercentage=0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}