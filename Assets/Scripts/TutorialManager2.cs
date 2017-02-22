using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager2 : MonoBehaviour
{
    public Transform Tutorial, tut1, tut2, mtrap, ready, skip;

    public PlayerController playerController;
    public LevelManager levelManager;
    public GameController gameController;

    void Start()
    {
        playerController = GameObject.Find("Mouse" + PlayerPrefs.GetInt("ActiveMouse") + "(Clone)").GetComponent<PlayerController>();

        if (SceneManager.GetActiveScene().name == "Level3" && playerController.mouseID != 2)
        {
            Tutorial.gameObject.SetActive(true);
            tut1.gameObject.SetActive(true);
            tut2.gameObject.SetActive(false);
            ready.gameObject.SetActive(false);
            playerController.canMove = false;
            gameController.stop = true;
            ready.gameObject.SetActive(false);
            mtrap.gameObject.SetActive(false);
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
        mtrap.gameObject.SetActive(true);
    }

    public void done()
    {
        Tutorial.gameObject.SetActive(false);
        mtrap.gameObject.SetActive(false);
        ready.gameObject.SetActive(true);
    }
}
