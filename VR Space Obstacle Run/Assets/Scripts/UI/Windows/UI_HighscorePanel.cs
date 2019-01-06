using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HighscorePanel : MonoBehaviour {

    public Text[] leaderboardScores;
    public Text[] leaderboardNames;

    private Score[] leaderboard;

    private void OnEnable()
    {
        UpdateLeaderboard();
    }

    private void UpdateLeaderboard()
    {
        leaderboard = GameManager.instance.highscores;

        for (int i = 0; i < leaderboard.Length; i++)
        {
            leaderboardScores[i].text = leaderboard[i].score.ToString();
            leaderboardNames[i].text = leaderboard[i].playerName;
        }
    }
}
