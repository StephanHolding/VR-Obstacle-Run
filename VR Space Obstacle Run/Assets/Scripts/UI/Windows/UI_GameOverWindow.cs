using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOverWindow : MonoBehaviour {

    public PlayerNameLetter[] playerNameLetters; 

    public void UpdateCurrentPlayername()
    {
        GameManager.instance.currentScore.playerName = GetInputPlayername();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    private string GetInputPlayername()
    {
        string toReturn = string.Empty;

        for (int i = 0; i < playerNameLetters.Length; i++)
        {
            toReturn += playerNameLetters[i].letter.text;
        }

        return toReturn;
    }
}
