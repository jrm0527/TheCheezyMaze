using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject mainMenuButton;
    public GameObject exitGameButton;
    public GameObject nextLevelButton;
    public Text LevelName;

    public StarManager starManager;
    private GameController gameController;
    private SettingManager settingManager;

    private int levelAmount = 10;
    public int currentLevel;
    public int timeLeft;
    public int mouseIdent;

    void Awake()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void Start()
    {
        restartButton.SetActive(false);
        mainMenuButton.SetActive(false);
        exitGameButton.SetActive(false);
        nextLevelButton.SetActive(false);
        //        checkCurrentLevel();
        mouseIdent = PlayerPrefs.GetInt("ActiveMouse");

        for (int i = 1; i <= levelAmount; i++)
        {
            if (SceneManager.GetActiveScene().name == "Level" + i)
            {
                currentLevel = i;
            }
        }
        LevelName.text = "Level " + currentLevel;
    }
    void Update()
    {
        if (gameController.gameOver == true)
        {
            restartButton.SetActive(true);
            mainMenuButton.SetActive(true);
            exitGameButton.SetActive(true);
        }
        if (gameController.gameWin == true)
        {
            nextLevelButton.SetActive(true);
            //            checkCurrentLevel();
            //SaveGame();
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Social.ReportProgress("CgkIj8GVwKMTEAIQBA", 100.0f, (bool success) =>
                {

                });
            }
        }
    }

    public void LoadScene (string name)
    {
        SceneManager.LoadScene(name);
    }

    public void exitGame ()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

/*    public void checkCurrentLevel()
    {
        for (int i = 1; i<=levelAmount;i++)
        {
            if (SceneManager.GetActiveScene().name == "Level" + i)
            {
                currentLevel = i;
//                SaveGame();
            }
        }
    }*/

    public void SaveGame()
    {
        int nextLevel = currentLevel + 1;
        int timeLeft = gameController.timeLeft;
        int star3 = starManager.Star3;
        int star2 = starManager.Star2;
        int star1 = starManager.Star1;

        PlayerPrefs.SetInt("Level" + currentLevel.ToString() + "3Star", star3);
        PlayerPrefs.SetInt("Level" + currentLevel.ToString() + "2Star", star2);
        PlayerPrefs.SetInt("Level" + currentLevel.ToString() + "1Star", star1);
        if (!PlayerPrefs.HasKey("Level" +currentLevel.ToString()+"_timeLeft_" + mouseIdent))
        {
            PlayerPrefs.SetInt("Level" + currentLevel.ToString() + "_timeLeft_" + mouseIdent, timeLeft);
        }
        if (PlayerPrefs.GetInt("Level" + currentLevel.ToString() + "_timeLeft_" + mouseIdent, timeLeft) < timeLeft)
        {
            PlayerPrefs.SetInt("Level" + currentLevel.ToString() + "_timeLeft_" + mouseIdent, timeLeft);
        }

        if (nextLevel<=levelAmount)
        {
            PlayerPrefs.SetInt("Level" +nextLevel.ToString() + "_" + mouseIdent, 1);
        }
    }
}
