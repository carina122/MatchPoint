using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
   public static ScoreBoard instance;

    public Text ScoreText;
    public Text HighScoreText;

    public void Start()
    {
        instance = this; // pointer to the player instance; used so other scripts can access the player
        UpdateScore(0);
        UpdateHighScore(0);
    }

    public void Update()
    {
        
    }

    public void UpdateScore(int newScoreValue)
    {
        ScoreText.text = "Score: " + newScoreValue;
    }

    public void UpdateHighScore(int newHighScoreValue)
    {
        HighScoreText.text = "Highscore: " + newHighScoreValue;
    }
}
