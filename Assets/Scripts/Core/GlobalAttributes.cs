public static class GlobalAttributes
{
    public static int currentLevel=0;

    public static void NextLevel()
    {
        currentLevel=(currentLevel+1)%LevelManager.GetInstance().levels.Count;
        //Get food and save it
    }
}
