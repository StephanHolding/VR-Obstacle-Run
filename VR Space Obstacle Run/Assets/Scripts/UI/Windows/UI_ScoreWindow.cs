using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreWindow : MonoBehaviour {

    public Text scoreText;

    private void OnEnable()
    {
        if (GameManager.instance != null)
        {
            scoreText.text = GameManager.instance.currentScore.score.ToString();
        }

    }
}
