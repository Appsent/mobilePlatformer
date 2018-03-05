using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public int LevelAmount = 6;
    public int CurrentLevel;
    public int NextLevel;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }
	public int Points { get; private set; }

    private GameManager()
    {
       
    }

    public void Reset()
    {
        Points = 0;
    }

    public void ResetPoints(int points)
    {
        Points = points;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    public void CheckCurrentLevel()
    {
        for (int i = 1; i < LevelAmount; i++)
        {
            if (SceneManager.GetActiveScene().name == "Level" + i)
            {
                CurrentLevel = i;

            }
        }
    }

    public void SaveMyGame()
    {
        NextLevel = CurrentLevel + 1;
        if (NextLevel < LevelAmount)
        {
            PlayerPrefs.SetInt("Level" + NextLevel.ToString(), 1);
            PlayerPrefs.SetInt("Level" + CurrentLevel.ToString() + "_points", Points);
        }
        else
        {
            PlayerPrefs.SetInt("Level" + CurrentLevel.ToString() + "_points", Points);
        }
    }
}
