using UnityEngine;

public static class GlobalAttributes
{
    public static int[] earnedFoods = new int[7] { 0, 0, 0, 0, 0, 0, 0 }; //Foods that we earned
    public static int currentLevel = 0; //Current level
    public static int lastEarnedFood; //The last food we earned

    public static void NextLevel()
    {
        lastEarnedFood=Random.Range(0,earnedFoods.Length);
        earnedFoods[lastEarnedFood]++;
    }

    public static int[] GetMatrix()
    {
        TextAsset targetFile = Resources.Load<TextAsset>("matrix"); //Load, read and return the .json data from resources folder
        matrixData data = JsonUtility.FromJson<matrixData>(targetFile.text);
        return data._matrix;
    }

    public static void SaveTheGame() //Saving progress
    {
        PlayerPrefs.SetInt("level",currentLevel);
        for(int i=0;i<earnedFoods.Length;i++)
            PlayerPrefs.SetInt("earned foods"+i,earnedFoods[i]);
    }

    public static void LoadTheGame() //Loading data
    {
        currentLevel=PlayerPrefs.GetInt("level");
        for(int i=0;i<earnedFoods.Length;i++)
            earnedFoods[i]=PlayerPrefs.GetInt("earned foods"+i);
    }
}

struct matrixData
{
    public int[] _matrix; //To deserialize
}