using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GameController : MonoBehaviour
{
    public Transform BacktoMenu, ready;

    public Text gameOverText;
    public Text timerText;
    public Text winText;
    public Text countText;
    public GameObject spawnPoint;
    public GameObject Mouse2Light;
    public GameObject Mouse1Light;
    public GameObject Mouse2;
    public GameObject Mouse1;
    public CheesePanel cheesePanelLg;
    public CheesePanel cheesePanelSm;

    public AudioClip gWin;
    public AudioClip gLose;
    public AudioClip cheeseRate;

    public PlayerController playerController;
    public LevelManager levelManager;
    
    public float timeRemaining;
    public bool stop = true;
    private float minutes;
    private float seconds;
    public int timesUp;

    public bool gameOver;
    public bool gameWin;
    public bool EndGame;

    public int cheeseLeft;
    public int timeLeft;
    GameObject tutorial = null;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Level9" || SceneManager.GetActiveScene().name == "Level10")
        {
            if (PlayerPrefs.GetInt("ActiveMouse") == 1)
            {
                GameObject Mouse = Instantiate(Mouse1Light) as GameObject;
                Mouse.transform.position = spawnPoint.transform.position;
                DestroyObject(spawnPoint);
            }
            if (PlayerPrefs.GetInt("ActiveMouse") == 2)
            {
                GameObject Mouse = Instantiate(Mouse2Light) as GameObject;
                Mouse.transform.position = spawnPoint.transform.position;
                DestroyObject(spawnPoint);
            }
        }
        if (SceneManager.GetActiveScene().name != "Level9" && SceneManager.GetActiveScene().name != "Level10")
        {
            if (PlayerPrefs.GetInt("ActiveMouse") == 1)
            {
                GameObject Mouse = Instantiate(Mouse1) as GameObject;
                Mouse.transform.position = spawnPoint.transform.position;
                DestroyObject(spawnPoint);
            }
            if (PlayerPrefs.GetInt("ActiveMouse") == 2)
            {
                GameObject Mouse = Instantiate(Mouse2) as GameObject;
                Mouse.transform.position = spawnPoint.transform.position;
                DestroyObject(spawnPoint);
            }
        }
    }

    void Start()
    {
        gameWin = false;
        gameOver = false;
        EndGame = false;
        gameOverText.text = "";
        winText.text = "";
        BacktoMenu.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name != "Level9" && SceneManager.GetActiveScene().name != "Level10")
        {
            playerController = GameObject.Find("Mouse" + PlayerPrefs.GetInt("ActiveMouse") + "(Clone)").GetComponent<PlayerController>();
        }
        if (SceneManager.GetActiveScene().name == "Level9" || SceneManager.GetActiveScene().name == "Level10")
        {
            playerController = GameObject.Find("Mouse" + PlayerPrefs.GetInt("ActiveMouse") + "Light(Clone)").GetComponent<PlayerController>();
        }

            if (SceneManager.GetActiveScene().name == "Level1" && playerController.mouseID != 2 || SceneManager.GetActiveScene().name == "Level3" && playerController.mouseID != 2)
        {
            ready.gameObject.SetActive(false);
        }
        else
        {
            ready.gameObject.SetActive(true);
        }
        stop = true;
        playerController.canMove = false;

        tutorial = GameObject.FindGameObjectWithTag("Tutorial");

        if (playerController.mouseID == 2)
        {
            GameObject[] cheeseR = GameObject.FindGameObjectsWithTag("Pickup");
            foreach (GameObject cheeseCR in cheeseR)
            {
                cheeseCR.GetComponent<CapsuleCollider>().radius = 8.0f;
            }
        }

        if (PlayerPrefs.HasKey("TimesUp"))
        {
            timesUp = PlayerPrefs.GetInt("TimesUp");
        }
        if (!PlayerPrefs.HasKey("TimesUp"))
        {
            timesUp = 0;
            PlayerPrefs.SetInt("TimesUp", timesUp);
        }
    }

    void Update()
    {
        GameObject[] cheese = GameObject.FindGameObjectsWithTag("Pickup");
        cheeseLeft = cheese.Length;
        countText.text = "Cheese Left: " + cheeseLeft;
        if (cheeseLeft == 0 && EndGame == false)
        {
            EndGame = true;
            endGame();

        }

        if (!stop)
        {
            timeRemaining -= Time.deltaTime;
        }

        minutes = Mathf.Floor(timeRemaining / 60);
        seconds = timeRemaining % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            minutes = 0;
            seconds = 0;
        }

        timerText.text = "Time Left: " + string.Format("{0:0}:{1:00}", minutes, seconds);
        if (timeRemaining < 0.0f)
        {
//            SoundManager.instance.PlaySingle(gLose);
//            SettingManager.instance.musicSource.Stop();
            GameOver();
            timesUp = timesUp + 1;
            PlayerPrefs.SetInt("TimesUp", timesUp);
            if (timesUp == 1)
            {
                Social.ReportProgress("CgkIj8GVwKMTEAIQBg", 100.0f, (bool success) =>
                {

                });
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stop = true;
            BacktoMenu.gameObject.SetActive(true);
            playerController.canMove = false;
        }
    }

    public void Cancel()
    {
        BacktoMenu.gameObject.SetActive(false);

        if (tutorial == null)
        {

            if (ready.gameObject.activeInHierarchy == false)
            {
                stop = false;
                playerController.canMove = true;
            }
        }
        if (tutorial != null)
        {
            if (tutorial.gameObject.activeInHierarchy == false)
            {
                if (ready.gameObject.activeInHierarchy == false)
                {
                    stop = false;
                    playerController.canMove = true;
                }
            }
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        stop = true;
        gameOver = true;
        EndGame = true;
        /*        if (playerController.trapSound.isPlaying)
                {
                    Loser.PlayDelayed(1);
                }*/
    }

    public void Ready()
    {
        playerController.canMove = true;
        stop = false;
        ready.gameObject.SetActive(false);
    }

    public void endGame()
    {
        stop = true;
        winText.text = "YOU FOUND THE CHEESE!";
        timeLeft = (int)timeRemaining;
        levelManager.SaveGame();
        StartCoroutine(CheeseRating());

        if (PlayerPrefs.GetInt("Level"  + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "3Star"))
        {
            Social.ReportProgress("CgkIj8GVwKMTEAIQAg", 100.0f, (bool success) =>
            {

            });
        }

        if (playerController.mouseID == 1)
        {
            if (PlayerPrefs.GetInt("Level1_timeLeft_1") > PlayerPrefs.GetInt("Level13Star") && PlayerPrefs.GetInt("Level2_timeLeft_1") > PlayerPrefs.GetInt("Level23Star") && PlayerPrefs.GetInt("Level3_timeLeft_1") > PlayerPrefs.GetInt("Level33Star") &&
                PlayerPrefs.GetInt("Level4_timeLeft_1") > PlayerPrefs.GetInt("Level43Star") && PlayerPrefs.GetInt("Level5_timeLeft_1") > PlayerPrefs.GetInt("Level53Star") && PlayerPrefs.GetInt("Level6_timeLeft_1") > PlayerPrefs.GetInt("Level63Star") &&
                PlayerPrefs.GetInt("Level7_timeLeft_1") > PlayerPrefs.GetInt("Level73Star") && PlayerPrefs.GetInt("Level8_timeLeft_1") > PlayerPrefs.GetInt("Level83Star") && PlayerPrefs.GetInt("Level9_timeLeft_1") > PlayerPrefs.GetInt("Level93Star") &&
                PlayerPrefs.GetInt("Level10_timeLeft_1") > PlayerPrefs.GetInt("Level103Star"))
            {
                Social.ReportProgress("CgkIj8GVwKMTEAIQAw", 100.0f, (bool success) =>
                {

                });
            }
        }
        if (playerController.mouseID == 2)
        {
            if (PlayerPrefs.GetInt("Level1_timeLeft_2") > PlayerPrefs.GetInt("Level13Star") && PlayerPrefs.GetInt("Level2_timeLeft_2") > PlayerPrefs.GetInt("Level23Star") && PlayerPrefs.GetInt("Level3_timeLeft_2") > PlayerPrefs.GetInt("Level33Star") &&
                PlayerPrefs.GetInt("Level4_timeLeft_2") > PlayerPrefs.GetInt("Level43Star") && PlayerPrefs.GetInt("Level5_timeLeft_2") > PlayerPrefs.GetInt("Level53Star") && PlayerPrefs.GetInt("Level6_timeLeft_2") > PlayerPrefs.GetInt("Level63Star") &&
                PlayerPrefs.GetInt("Level7_timeLeft_2") > PlayerPrefs.GetInt("Level73Star") && PlayerPrefs.GetInt("Level8_timeLeft_2") > PlayerPrefs.GetInt("Level83Star") && PlayerPrefs.GetInt("Level9_timeLeft_2") > PlayerPrefs.GetInt("Level93Star") &&
                PlayerPrefs.GetInt("Level10_timeLeft_2") > PlayerPrefs.GetInt("Level103Star"))
            {
                Social.ReportProgress("CgkIj8GVwKMTEAIQAw", 100.0f, (bool success) =>
                {

                });
            }
        }
    }
    IEnumerator CheeseRating()
    {
        SettingManager.instance.musicSource.Stop();
        cheesePanelLg.gameObject.SetActive(true);
        SettingManager.instance.PlaySingle(gWin);
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            yield return new WaitForSeconds(7);
        } else
        {
            yield return new WaitForSeconds(1);
        }
        if (PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "1Star"))
        {
            cheesePanelLg.Cheese1.gameObject.SetActive(true);
            SettingManager.instance.PlaySingle(cheeseRate);
            if (PlayerPrefs.GetInt("sound") == 1)
            {
                yield return new WaitForSeconds(0.9f);
            } else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "2Star"))
        {
            cheesePanelLg.Cheese2.gameObject.SetActive(true);
            SettingManager.instance.PlaySingle(cheeseRate);
            if (PlayerPrefs.GetInt("sound") == 1)
            {
                yield return new WaitForSeconds(0.9f);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "3Star"))
        {
            cheesePanelLg.Cheese3.gameObject.SetActive(true);
            SettingManager.instance.PlaySingle(cheeseRate);
            if (PlayerPrefs.GetInt("sound") == 1)
            {
                yield return new WaitForSeconds(0.9f);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        SettingManager.instance.musicSource.Play();
        yield return new WaitForSeconds(1.5f);
        cheesePanelSm.gameObject.SetActive(true);
        cheesePanelLg.gameObject.SetActive(false);
        gameOver = true;
        gameWin = true;
        if (PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "1Star"))
        {
            cheesePanelSm.Cheese1.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "2Star"))
        {
            cheesePanelSm.Cheese2.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "_timeLeft_" + levelManager.mouseIdent) > PlayerPrefs.GetInt("Level" + levelManager.currentLevel.ToString() + "3Star"))
        {
            cheesePanelSm.Cheese3.gameObject.SetActive(true);
        }
        cheesePanelLg.gameObject.SetActive(false);
    }
}