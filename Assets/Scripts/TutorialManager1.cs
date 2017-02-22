using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager1 : MonoBehaviour
{
    public Transform Tutorial, tut1, tut2, tut3, tut4, tut5, img2, img3, img4, ready, skip;

    public PlayerController playerController;
    public LevelManager levelManager;
    public GameController gameController;

    void Start()
    {
        playerController = GameObject.Find("Mouse" + PlayerPrefs.GetInt("ActiveMouse") + "(Clone)").GetComponent<PlayerController>();
        if (SceneManager.GetActiveScene().name == "Level1" && playerController.mouseID != 2)
        {
            Tutorial.gameObject.SetActive(true);
            tut1.gameObject.SetActive(true);
            tut2.gameObject.SetActive(false);
            tut3.gameObject.SetActive(false);
            tut4.gameObject.SetActive(false);
            tut5.gameObject.SetActive(false);
            img2.gameObject.SetActive(false);
            img3.gameObject.SetActive(false);
            img4.gameObject.SetActive(false);
            playerController.canMove = false;
            gameController.stop = true;
            ready.gameObject.SetActive(false);
            skip.gameObject.SetActive(true);
        }
        if (playerController.mouseID == 2)
        {
            Tutorial.gameObject.SetActive(false);
            ready.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (playerController == null)
        {
            playerController = GameObject.Find("Mouse" + PlayerPrefs.GetInt("ActiveMouse") + "(Clone)").GetComponent<PlayerController>();
        }
    }

    public void second()
    {
        tut1.gameObject.SetActive(false);
        tut2.gameObject.SetActive(true);
        img2.gameObject.SetActive(true);
    }

    public void third()
    {
        tut2.gameObject.SetActive(false);
        tut3.gameObject.SetActive(true);
        img2.gameObject.SetActive(false);
        img3.gameObject.SetActive(true);
    }

    public void fourth()
    {
        tut3.gameObject.SetActive(false);
        tut4.gameObject.SetActive(true);
        img3.gameObject.SetActive(false);
        img4.gameObject.SetActive(true);
    }
    public void fifth()
    {
        tut4.gameObject.SetActive(false);
        tut5.gameObject.SetActive(true);
        img4.gameObject.SetActive(false);
    }

    public void done()
    {
        Tutorial.gameObject.SetActive(false);
        ready.gameObject.SetActive(true);
    }
}
