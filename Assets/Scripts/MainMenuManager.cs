using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MainMenuManager : MonoBehaviour
{
    public Transform mainMenu, Credits, levelMenu, Spacer, confirmation, TestWorldMenu, TestWorldButtons, kitchenMenu, kitchenButtons, nextButton, previousButton, charSelect,
        high1, high2, m2Locked, mouse2, mouseLocked, img2Locked, img2Unlocked, img1, description1, description2, GPGRequest;

    [System.Serializable]

    public class Level
    {
        public int worldNum;
        public string LevelName;
        public string LevelText;
        public int UnLocked;
        public bool IsInteractable;
    }

    public GameObject levelButton;
    public List<Level> LevelList;
    private LevelButton button;
    public AudioSource clickSound;
    public static string version;
    public Text versionText;
    public int gpg;

    void Awake()
    {
        mainMenu.gameObject.SetActive(true);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("GPG"))
        {
            GPGRequest.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("GPG") == 1)
        {
            GPGRequest.gameObject.SetActive(false);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) =>
            {

            });
            gpg = 1;
        }
        else
        {
            gpg = 0;
        }

        Credits.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(false);
        charSelect.gameObject.SetActive(false);
        versionText.text = "Version - " + Application.version;
        
    }

    public void gpgStart()
    {
        GPGRequest.gameObject.SetActive(false);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {

        });
        PlayerPrefs.SetInt("GPG", 1);
        gpg = 1;
    }

    public void noGPG()
    {
        GPGRequest.gameObject.SetActive(false);
        PlayerPrefs.SetInt("GPG", 0);
        gpg = 0;
        PlayGamesPlatform.Instance.SignOut();
    }

    //starts a new game on Level 1 and asks to erase all previously saved data
    public void NewGame()
    {
        if (PlayerPrefs.HasKey("Level1_timeLeft_1"))
        {
            confirmation.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Level1");
            PlayerPrefs.SetInt("ActiveMouse", 1);
        }
    }

    //cancels the new game dialogue box preventing data deletion.
    public void denyErase()
    {
        confirmation.gameObject.SetActive(false);
    }

    public void changeGPG()
    {
        GPGRequest.gameObject.SetActive(true);
    }

    //confirms data can be erased.
    public void Confirm()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("GPG", gpg);
        PlayerPrefs.SetInt("ActiveMouse", 1);
        SceneManager.LoadScene("Level1");
    }

    //exits game
    public void exitGame()
    {
        PlayGamesPlatform.Instance.SignOut();
        Application.Quit();
    }

    //loads character select screen
    public void Continue()
    {
        levelMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
        charSelect.gameObject.SetActive(true);

        if (!PlayerPrefs.HasKey("ActiveMouse"))
        {
            high2.gameObject.SetActive(false);
            high1.gameObject.SetActive(true);
            PlayerPrefs.SetInt("ActiveMouse", 1);
        }

        if (PlayerPrefs.GetInt("ActiveMouse") == 1)
        {
            high2.gameObject.SetActive(false);
            high1.gameObject.SetActive(true);
            description1.gameObject.SetActive(true);
            description2.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt("ActiveMouse") == 2)
        {
            high2.gameObject.SetActive(true);
            high1.gameObject.SetActive(false);
            description1.gameObject.SetActive(false);
            description2.gameObject.SetActive(true);
        }


        if (!PlayerPrefs.HasKey("Level10_timeLeft_1")) //locks 2nd character
        {
            m2Locked.gameObject.SetActive(true);
            mouse2.gameObject.SetActive(false);
            img2Locked.gameObject.SetActive(true);
            img2Unlocked.gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("Level10_timeLeft_1")) //unlocks 2nd character
        {
            m2Locked.gameObject.SetActive(false);
            mouse2.gameObject.SetActive(true);
            img2Locked.gameObject.SetActive(false);
            img2Unlocked.gameObject.SetActive(true);
        }
    }
    
    //loads level menu from character select screen
    public void LevelMenu()
    {
        FillList();
        levelMenu.gameObject.SetActive(true);
        Kitchen();
        mainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
        charSelect.gameObject.SetActive(false);
    }

    //displays dialogue telling player how to unlcok
    public void MouseLocked (bool clicked)
    {
        if (clicked == true)
        {
            mouseLocked.gameObject.SetActive(true);
        } else {
            mouseLocked.gameObject.SetActive(false);
        }
    }

    //Sets character to mouse1
    public void setMouse1()
    {
        PlayerPrefs.SetInt("ActiveMouse", 1);
        high2.gameObject.SetActive(false);
        high1.gameObject.SetActive(true);
        description1.gameObject.SetActive(true);
        description2.gameObject.SetActive(false);

        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        foreach (GameObject buttons in allButtons)
        {
            DestroyObject(buttons);
        }
    }

    public void setMouse2()
    {
        PlayerPrefs.SetInt("ActiveMouse", 2);
        high2.gameObject.SetActive(true);
        high1.gameObject.SetActive(false);
        description2.gameObject.SetActive(true);
        description1.gameObject.SetActive(false);

        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        foreach (GameObject buttons in allButtons)
        {
            DestroyObject(buttons);
        }
    }

    //loads the kitchen world on LevelMenu
    public void Kitchen()
    {
        kitchenMenu.gameObject.SetActive(true);
        TestWorldMenu.gameObject.SetActive(false);
        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false); //set true when needing next world
    }

    //loads the TestWorld on levelMenu
    public void TestWorld()
    {
        kitchenMenu.gameObject.SetActive(false);
        TestWorldMenu.gameObject.SetActive(true);
        previousButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }

    //click sound
    public void Click()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            clickSound.Play();
        }
    }

    //loads credit menu
        public void CreditsMenu()
    {
        Credits.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(false);
        charSelect.gameObject.SetActive(false);
    }

    //returns to main menu
    public void Return()
    {
//        Credits.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
//        levelMenu.gameObject.SetActive(false);
//        charSelect.gameObject.SetActive(false);

        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        foreach (GameObject buttons in allButtons)
        {
            DestroyObject(buttons);
        }
    }

    //populates world with buttons corresponding to levels
    void FillList()
    {
        foreach(var level in LevelList)
        {
            GameObject newButton = Instantiate(levelButton) as GameObject;
            LevelButton button = newButton.GetComponent<LevelButton>();
            button.LevelText.text = level.LevelName;
            button.levelIdent = level.LevelText;
            button.world = level.worldNum;

            if (PlayerPrefs.GetInt("Level" + button.levelIdent + "_" + PlayerPrefs.GetInt("ActiveMouse")) == 1) //set int to 1 which tells the button to unlock
            {
                level.UnLocked = 1;
                level.IsInteractable = true;
            }

            button.unlocked = level.UnLocked;
            button.GetComponent<Button>().interactable = level.IsInteractable;
            button.GetComponent<Button>().onClick.AddListener(() => loadLevel("Level" + button.levelIdent));

            if (PlayerPrefs.GetInt("Level" + button.levelIdent + "_timeLeft_" + PlayerPrefs.GetInt("ActiveMouse")) > (PlayerPrefs.GetInt("Level" + button.levelIdent + "1Star"))) //Gets time based on character
            {
                button.Star1.SetActive(true);
            }
            if (PlayerPrefs.GetInt("Level" + button.levelIdent + "_timeLeft_" + PlayerPrefs.GetInt("ActiveMouse")) > (PlayerPrefs.GetInt("Level" + button.levelIdent + "2Star"))) //Gets time based on character
            {
                button.Star2.SetActive(true);
            }
            if (PlayerPrefs.GetInt("Level" + button.levelIdent + "_timeLeft_" + PlayerPrefs.GetInt("ActiveMouse")) > (PlayerPrefs.GetInt("Level" + button.levelIdent + "3Star"))) //Gets time based on character
            {
                button.Star3.SetActive(true);
            }
            if (button.world == 1)
            {
                newButton.transform.SetParent(kitchenButtons, false);
            }
            if (button.world == 2)
            {
                newButton.transform.SetParent(TestWorldButtons, false); //only need when starting second world
            }
        }
        SaveAll();
    }

    void SaveAll()
    {
        if (PlayerPrefs.HasKey("Level1"))
        {
            return;
        }
        else
        {
            GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");
            foreach (GameObject buttons in allButtons)
            {
                LevelButton button = buttons.GetComponent<LevelButton>();
                PlayerPrefs.SetInt("Level" + button.LevelText.text, button.unlocked);
            }
        }
    }

    public void loadLevel(string value)
    {
        SceneManager.LoadScene(value);
    }
}
