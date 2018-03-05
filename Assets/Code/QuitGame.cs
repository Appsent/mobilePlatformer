using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

    public CanvasGroup uiCanvasGroup;
    public CanvasGroup confirmQuitCanvasGroup;


    public void Start()
    {
        DoConfirmQuitNo();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DoQuit();
            Time.timeScale = 0;
        }
    }

    public void DoConfirmQuitNo()
    {
        Debug.Log("Back to the game");

        //enable the normal ui
        uiCanvasGroup.alpha = 1;
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;

        //disable the confirmation quit ui
        confirmQuitCanvasGroup.alpha = 0;
        confirmQuitCanvasGroup.interactable = false;
        confirmQuitCanvasGroup.blocksRaycasts = false;

        Time.timeScale = 1;
    }

    public void DoConfirmQuitYes()
    {
        Debug.Log("Ok bye bye");
        Application.Quit();
    }

    public void DoQuit()
    {
        Debug.Log("Check form quit confirmation");
            //reduce the visibility of normal UI, and disable all interraction
            uiCanvasGroup.alpha = 0.8f;
            uiCanvasGroup.interactable = false;
            uiCanvasGroup.blocksRaycasts = false;

            //enable interraction with confirmation gui and make visible
            confirmQuitCanvasGroup.alpha = 1;
            confirmQuitCanvasGroup.interactable = true;
            confirmQuitCanvasGroup.blocksRaycasts = true;

    }
}
