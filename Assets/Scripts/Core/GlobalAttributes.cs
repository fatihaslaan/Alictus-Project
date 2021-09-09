using UnityEngine;

public static class GlobalAttributes
{
    public static int currentLevel=2;

    public static void NextLevel()
    {
        currentLevel=(currentLevel+1)%LevelManager.GetInstance().levels.Count;
        //Get food and save it
    }

    public static int[] GetMatrix()
    {
        TextAsset targetFile = Resources.Load<TextAsset>("matrix"); //Load, read and return the .json data from resources folder
        matrixData data=JsonUtility.FromJson<matrixData>(targetFile.text);
        return data._matrix;
    }
}

struct matrixData
{
    public int[] _matrix; //To deserialize
}