using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AdiveryUnity;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{

    public static AdManager instance;
	
    public string AppID = "59c36ce3-7125-40a7-bd34-144e6906c796";
    public string BannerID = "a355be22-970a-46b8-bc52-f0a59c4ded05";
    public string InterstitialID = "38b301f2-5e0c-4776-b671-c6b04a612311";
    public string RewardID = "16414bae-368e-4904-b259-c5b89362206d";

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }
    BannerAd bannerAd;
    AdiveryListener listener;
    public void Start()
    {
        Adivery.Configure(AppID);

        Adivery.PrepareInterstitialAd(InterstitialID);
        Adivery.PrepareRewardedAd(RewardID);
        listener = new AdiveryListener();

        listener.OnError += OnError;
        listener.OnInterstitialAdLoaded += OnInterstitialAdLoaded;
        listener.OnRewardedAdLoaded += OnRewardedLoaded;
        listener.OnRewardedAdClosed += OnRewardedClosed;
		
        bannerAd = new BannerAd(BannerID, BannerAd.TYPE_BANNER, BannerAd.POSITION_BOTTOM);
        bannerAd.OnAdLoaded += OnBannerAdLoaded;
        bannerAd.LoadAd();
        Adivery.AddListener(listener);
    }

    private void OnBannerAdLoaded(object sender, EventArgs e)
    {
    }

    private void OnRewardedClosed(object sender, AdiveryReward reward)
    {
        if (reward.IsRewarded){
            // Implrement getRewardAmount yourself
        }
    }

    public void ShowRewardedAd()
    {
        if (Adivery.IsLoaded(RewardID)){
            Adivery.Show(RewardID);
        }
    }
    private void OnRewardedLoaded(object sender, string e)
    {
    }

    private void OnInterstitialAdLoaded(object sender, string e)
    {
    }

    private void OnError(object sender, AdiveryError e)
    {
    }

    public void ShowInterstitial()
    {
        if (Adivery.IsLoaded(InterstitialID))
        {
            Adivery.Show(InterstitialID);
        }
    }
    public void HideBanner()
    {
        this.bannerAd.Hide();
    }

    public void ShowBanner()
    {
        this.bannerAd.Show();
    }
}
