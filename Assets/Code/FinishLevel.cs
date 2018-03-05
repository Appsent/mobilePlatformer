using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public GameObject completeMenuPanel;
    public GameObject chestAnim;
    private Animator anim;
    private Animator anim1;

    void Awake()
    {
        Time.timeScale = 1;
        anim1 = completeMenuPanel.GetComponent<Animator>();
        anim = chestAnim.GetComponent<Animator>();
        anim.enabled = false;
        anim1.enabled = false;
    }

    public void ChestAnim()
    {
        anim.enabled = true;
        anim.Play("ChestAnim");
    }

    public void CompleteLevel()
    {
        anim1.enabled = true;
        anim1.Play("CompleteMenuSlideIn");
      
        Time.timeScale = 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;
        ChestAnim();
        //CompleteLevel();
        GameManager.Instance.SaveMyGame();
    }

    public void NextLevel()
    {
        if (GameManager.Instance.NextLevel < GameManager.Instance.LevelAmount)
        {
            SceneManager.LoadScene("Level" + GameManager.Instance.NextLevel);
        }
        else
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
}
