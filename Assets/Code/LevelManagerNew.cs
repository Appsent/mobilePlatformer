using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class LevelManagerNew : MonoBehaviour
{
    [Serializable]
    public class Level
    {
        public string LevelText;
        public int Unlocked;
        public bool IsInteractable;

        public Button.ButtonClickedEvent OnClickEvent;
    }

    public GameObject button;
    public Transform Spacer;
    public List<Level> LevelList;

    public bool glideEnabled;

    void Start()
    {
        //DeleteAll();
        FillList();
    }

    void FillList()
    {

        foreach (var level in LevelList)
        {
            GameObject newbutton = Instantiate(button) as GameObject;
            LevelButtonNew levelButtonNew = newbutton.GetComponent<LevelButtonNew>();
            levelButtonNew.LevelText.text = level.LevelText;

            if (PlayerPrefs.GetInt("Level" + levelButtonNew.LevelText.text) == 1)
            {
                level.Unlocked = 1;
                level.IsInteractable = true;
            }

            if (level.Unlocked == 0)
            {
                levelButtonNew.Locked.SetActive(true);
            }

            else
            {
                levelButtonNew.Locked.SetActive(false);
            }

            levelButtonNew.unlocked = level.Unlocked;
            levelButtonNew.GetComponent<Button>().interactable = level.IsInteractable;
            levelButtonNew.GetComponent<Button>().onClick.AddListener(() => loadLevels("Level" + levelButtonNew.LevelText.text));

            if(PlayerPrefs.GetInt("Level" + levelButtonNew.LevelText.text + "_points") > (15+(int.Parse(levelButtonNew.LevelText.text) * 200f)))
            {
                levelButtonNew.Star1.SetActive(true);
                levelButtonNew.Star2.SetActive(false);
                levelButtonNew.Star3.SetActive(false);
            }
            if (PlayerPrefs.GetInt("Level" + levelButtonNew.LevelText.text + "_points") > (1000 + (int.Parse(levelButtonNew.LevelText.text) * 200f)))
            {
                levelButtonNew.Star1.SetActive(false);
                levelButtonNew.Star2.SetActive(true);
                levelButtonNew.Star3.SetActive(false);
            }

            if (PlayerPrefs.GetInt("Level" + levelButtonNew.LevelText.text + "_points") > (2000 + (int.Parse(levelButtonNew.LevelText.text) * 200f)))
            {
                levelButtonNew.Star1.SetActive(false);
                levelButtonNew.Star2.SetActive(false);
                levelButtonNew.Star3.SetActive(true);
            }

            newbutton.transform.SetParent(Spacer, false);            
        }

        SaveAll();
    }

    void SaveAll()
    {
        if (PlayerPrefs.HasKey("Level1"))
        {
            return;
        }
        else
        {
            GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
            foreach (GameObject buttons in allButtons)
            {
                LevelButtonNew button = buttons.GetComponent<LevelButtonNew>();
                PlayerPrefs.SetInt("Level" + button.LevelText.text, button.unlocked);
            }
        }
    }

    void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public void loadLevels(string value)
    {
        SceneManager.LoadScene(value);
    }
}