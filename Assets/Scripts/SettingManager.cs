using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    Toggle SoundFX;
    Toggle Music;

    public AudioSource fxSource;
    public AudioSource musicSource;
    public static SettingManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public int music;
    public int sound;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "GameMenu")
        {
            Music = GameObject.FindWithTag("MusicToggle").GetComponent<Toggle>();
            SoundFX = GameObject.FindWithTag("SoundToggle").GetComponent<Toggle>();

            if (PlayerPrefs.HasKey("music"))
            {
                music = PlayerPrefs.GetInt("music");
                if (music == 1)
                {
                    Music.isOn = true;
                }
                if (music == 0)
                {
                    Music.isOn = false;
                }

                sound = PlayerPrefs.GetInt("sound");
                if (sound == 1)
                {
                    SoundFX.isOn = true;
                }
                if (sound == 0)
                {
                    SoundFX.isOn = false;
                }
            }
            else
            {
                music = 1;
                sound = 1;
            }
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameMenu")
        {
            GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");

            if (menu == true)
            {
                Music = GameObject.FindWithTag("MusicToggle").GetComponent<Toggle>();
                SoundFX = GameObject.FindWithTag("SoundToggle").GetComponent<Toggle>();
                if (Music.isOn == true)
                {
                    musicSource.mute = false;
                    music = 1;
                    PlayerPrefs.SetInt("music", music);
                }
                else
                {
                    musicSource.mute = true;
                    music = 0;
                    PlayerPrefs.SetInt("music", music);
                }
                if (SoundFX.isOn == true)
                {
                    fxSource.mute = false;
                    sound = 1;
                    PlayerPrefs.SetInt("sound", sound);
                }
                else
                {
                    fxSource.mute = true;
                    sound = 0;
                    PlayerPrefs.SetInt("sound", sound);
                }
            }
        }
    }

    public void PlaySingle (AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
    }

    public void RandomizeSfx (params AudioClip [] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        fxSource.pitch = randomPitch;
        fxSource.clip = clips[randomIndex];
        fxSource.Play();
    }
}
