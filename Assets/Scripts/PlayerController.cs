using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float turn;
    public float turnSpeed;
    public int mouseID;
    public int timesTrapped;

    public AudioSource chompSound;
    public AudioClip stepSound;
    public AudioClip squeak;
    public AudioClip squeak2;
    public AudioClip trapSound;
    public GameObject character;
    public GameObject pickup;
    public GameObject pickup2;

    private Animator anim;
    private Rigidbody rb;

    public bool canMove = true;
    private GameController gameController;

    public TouchPad touchPad;

    Vector3 dir = Vector3.zero;

    //    private Vector2 touchOrigin = -Vector2.one;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
        touchPad = GameObject.Find("TouchPad").GetComponent<TouchPad>();

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        character = GameObject.FindGameObjectWithTag("Player");
        pickup = null;
        pickup2 = null;

        if (PlayerPrefs.HasKey("TimesTrapped"))
        {
            timesTrapped = PlayerPrefs.GetInt("TimesTrapped");
        }
        if (!PlayerPrefs.HasKey("TimesTrapped"))
        {
            timesTrapped = 0;
            PlayerPrefs.SetInt("TimesTrapped", timesTrapped);
        }
    }

    void Update()
    {

        dir.x = touchPad.Horizontal();
        dir.z = touchPad.Vertical();
        turn = touchPad.Horizontal();

        if (touchPad == null)
        {
            touchPad = GameObject.Find("TouchPad").GetComponent<TouchPad>();
        }
    }

    void FixedUpdate()
    {
        if (gameController.EndGame == true)
        {
            canMove = false;
            rb.velocity = Vector3.zero;
            anim.SetInteger("AnimPar", 0);
        }

        if (canMove == true)
        {
             dir = transform.forward * dir.z * speed;
             rb.velocity = dir;

            //            rb.AddForce(dir * speed); //remove when done testing

            if (dir.magnitude > 1)
                dir.Normalize();

            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);

            if (touchPad.Vertical() != 0)
            {
                anim.SetInteger("AnimPar", 1);
            }
            else
            {
                anim.SetInteger("AnimPar", 0);
            }
            if (touchPad.Horizontal() < -0.3)
            {
                anim.SetInteger("AnimPar", 2);
            }
            if (touchPad.Horizontal() > 0.3)
            {
                anim.SetInteger("AnimPar", 3);
            }

                if (!SettingManager.instance.fxSource.isPlaying)
            {
                if (dir.z != 0)
                {
                    SettingManager.instance.fxSource.PlayDelayed(7);
                    SettingManager.instance.RandomizeSfx(squeak, squeak2);
                }
            }
        }

        if (canMove == false)
        {
            rb.velocity = Vector3.zero;
            anim.SetInteger("AnimPar", 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (mouseID == 2)
        {
            if (other.gameObject.CompareTag("Pickup") && (canMove == true))
            {

                if (pickup == null)
                {
                    pickup = other.gameObject;
                    StartCoroutine(moveToward(other.transform.position, 0.2f));
                }
                else
                {
                    pickup2 = other.gameObject;
                    StartCoroutine(moveToward2(other.transform.position, 0.2f));
                }
                if (pickup2 == pickup)
                {
                    pickup2 = null;
                }
            }
        }

            if (mouseID == 1)
            {
                if (other.gameObject.CompareTag("Pickup") && (canMove == true))
                {
                    other.gameObject.SetActive(false);
                    if (PlayerPrefs.GetInt("sound") == 1)
                    {
                        chompSound.Play();
                    }
                }
            }
        
        if (other.gameObject.CompareTag("Mousetrap") && (canMove == true))
        {
            gameController.GameOver();
            canMove = false;
            SettingManager.instance.PlaySingle(trapSound);
            timesTrapped = timesTrapped + 1;
            PlayerPrefs.SetInt("TimesTrapped", timesTrapped);
            if (PlayerPrefs.GetInt("TimesTrapped") == 1)
            {
                Social.ReportProgress("CgkIj8GVwKMTEAIQBQ", 100.0f, (bool success) =>
                {

                });
            }
        }
    }
    IEnumerator moveToward(Vector3 position, float time)
    {
        Vector3 start = pickup.transform.position;
        Vector3 end = character.transform.position;
        float t = 0.1f;

        while (pickup != null)
        {
            yield return null;
            t += Time.deltaTime / time;
            pickup.transform.position = Vector3.Lerp(start, end, t);
            if (pickup.transform.position == end)
            {
                Destroy(pickup);
                pickup = null;
                if (PlayerPrefs.GetInt("sound") == 1)
                {
                    chompSound.Play();
                }
            }
        }
    }

    IEnumerator moveToward2(Vector3 position, float time)
    {
        Vector3 start = pickup2.transform.position;
        Vector3 end = character.transform.position;
        float t = 0.1f;

        while (pickup2 != null)
        {
            yield return null;
            t += Time.deltaTime / time;
            pickup2.transform.position = Vector3.Lerp(start, end, t);
            if (pickup2.transform.position == end)
            {
                Destroy(pickup2);
                pickup2 = null;
                if (PlayerPrefs.GetInt("sound") == 1)
                {
                    chompSound.Play();
                }
            }
        }
    }
}
