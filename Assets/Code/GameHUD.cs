using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {

    public Text Level;
    public Text points;

    public void Update()
    {
        Level.text = "Level " + GameManager.Instance.CurrentLevel;
        points.text = "Points: " + GameManager.Instance.Points;
    }
}
