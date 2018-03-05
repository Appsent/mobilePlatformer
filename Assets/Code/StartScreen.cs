using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    public void loadLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void loadStartScene()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void loadInfoScene()
    {
        SceneManager.LoadScene("InfoScene");
    }

    public void loadSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level" + GameManager.Instance.CurrentLevel);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level" + GameManager.Instance.NextLevel);
    }
}
