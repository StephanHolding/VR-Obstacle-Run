using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool paused;

    [Header("VR Hardware")]
    public SteamVR_Input_Sources activeInput;
    public VR_Controller activeController;

    [SerializeField]
    private VR_Controller leftController;
    [SerializeField]
    private VR_Controller rightController;

    [Header("Score")]
    public Score currentScore;
    public Score[] highscores;

    private float scoreTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }


        if (activeController == null)
        {
            activeController = rightController;
            activeInput = SteamVR_Input_Sources.RightHand;
        }

        if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/VR Space Obstacle Run/Highscores.sav"))
        highscores = (Score[])DataManager.instance.LoadFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/VR Space Obstacle Run", "Highscores.sav", DataManager.SerializationMode.Binary);
    }

    private void Update()
    {
        //Sets a controller to the active controller if a button on that controller has been pressed.
        activeInput = GetActiveController();

        if (!paused)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1)
            {
                AddToScore(1);
                scoreTimer = 0;
            }
        }
    }

    public SteamVR_Input_Sources GetActiveController()
    {
        if (SteamVR_Input.Spaceship.inActions.SwitchController.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            activeController = rightController;
            return SteamVR_Input_Sources.RightHand;
        }
        else if (SteamVR_Input.Spaceship.inActions.SwitchController.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            activeController = leftController;
            return SteamVR_Input_Sources.LeftHand;
        }

        return activeInput;
    }

   
    public void TogglePause(bool toggle)
    {
        ObstacleManager.instance.MoveObstacles(toggle ? false : true);

        if (toggle)
            UIManager.instance.ShowWindow("Main Menu");
        else
            UIManager.instance.CloseAllWindows();

        Ship.playerShip.LockControls(toggle);
        paused = toggle;
    }

    public void GameOver()
    {
        Ship.playerShip.LockControls(true);
        Ship.playerShip.SetInteractionMode(InteractionHandler.InteractionMode.UI);
        UIManager.instance.ShowWindow("Game Over Menu");
        ObstacleManager.instance.MoveObstacles(false);
        paused = true;
        AddScoreToHighscores(currentScore);
    }

    public void AddToScore(int toAdd)
    {
        currentScore.score += toAdd;
    }

    private void OnApplicationQuit()
    {
        DataManager.instance.SaveFiles(highscores, System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/VR Space Obstacle Run", "Highscores.sav", DataManager.SerializationMode.Binary);
    }

    /// <summary>
    /// Inserts the current score into the leaderboard, provided that the current score is high enough.
    /// </summary>
    /// <param name="currentScore"></param>
    private void AddScoreToHighscores(Score currentScore)
    {
        for (int i = 0; i < highscores.Length; i++)
        {
            if (currentScore.score > highscores[i].score)
            {
                for (int j = highscores.Length - 1; j <= i; j--)
                {
                    Score temp = null;

                    temp = highscores[j];
                    highscores[j] = null;

                    if (j + 1 < highscores.Length)
                    {
                        highscores[j + 1] = temp;
                    }
                }

                highscores[i] = currentScore;
            }
        }
    }
}
