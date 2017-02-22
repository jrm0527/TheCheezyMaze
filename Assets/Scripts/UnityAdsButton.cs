using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class UnityAdsButton : MonoBehaviour
{
    public AdButton adButton;
    public int ads;

    void Awake()
    {
        if (PlayerPrefs.HasKey("AdsWatched"))
            {
            ads = PlayerPrefs.GetInt("AdsWatched");
        }
        if (!PlayerPrefs.HasKey("AdsWatched"))
        {
            ads = 0;
            PlayerPrefs.SetInt("AdsWatched", ads);
        }
    }

    public void achievements()
    {
        Social.ShowAchievementsUI();
    }

    IEnumerator Start()
    {
#if !UNITY_ADS // If the Ads service is not enabled...
        if (Advertisement.isSupported) { // If runtime platform is supported...
            Advertisement.Initialize(gameId, enableTestMode); // ...initialize.
        }
#endif

        // Wait until Unity Ads is initialized,
        //  and the default ad placement is ready.
        while (!Advertisement.isInitialized || !Advertisement.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

        void Update()
    {
        adButton.AdText.text = Advertisement.IsReady() ? "Show Ad" : "Waiting...";
    }

    public void adStart()
    {
        Advertisement.Show();
        ads = ads+1;
        PlayerPrefs.SetInt("AdsWatched", ads);
        if (ads == 1)
        {
            Social.ReportProgress("CgkIj8GVwKMTEAIQAQ", 100.0f, (bool success) =>
            {

            });
        }
        if (ads == 10)
        {
            Social.ReportProgress("CgkIj8GVwKMTEAIQBw", 100.0f, (bool success) =>
            {

            });
        }
        if (ads == 20)
        {
            Social.ReportProgress("CgkIj8GVwKMTEAIQCA", 100.0f, (bool success) =>
            {

            });
        }

    }


    }

/*    void OnGUI()
    {
        Rect buttonRect = new Rect(10, 10, 150, 50);
        AdButton.text = Advertisement.IsReady() ? "Show Ad" : "Waiting...";

        if (GUI.Button(buttonRect, buttonText))
        {
            Advertisement.Show();
        }
    }*/
