using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Unity.Services.Mediation;
using Unity.Services.Core;
using System;

public enum AdType
{
    BANNER,
    SKIPPABLE,
    REWARDED
}

public class AdsManager : MonoBehaviour//, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance;

    public string androidID = "4542053", iOSID = "4542052";
    public string androidRewarded = "Rewarded_Android", androidSkippable = "Interstitial_Android", androidBanner = "Banner_Android";
    public string iOSRewarded = "Rewarded_iOS", iOSSkippable = "Interstitial_iOS", iOSBanner = "Banner_iOS";
    string adID, rAd, iAd, bAd;


    bool testMode = false;

    AdType currentAdType = AdType.BANNER;
    IRewardedAd rewardedAd;
    IInterstitialAd interstitialAd;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        //await UnityServices.InitializeAsync();
#if UNITY_ANDROID
        adID = androidID;
        rAd = androidRewarded;
        iAd = androidSkippable;
        bAd = androidBanner;
#elif UNITY_IOS
        adID = iOSID;
        rAd = iOSRewarded;
        iAd = iOSSkippable;
        bAd = iOSBanner;
#else
        return;
#endif
        try
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetGameId(adID);
            await UnityServices.InitializeAsync(initializationOptions);

            InitializationComplete();
        }
        catch (Exception e)
        {
            InitializationFailed(e);
        }
    }

    public void SetupAd()
    {
        //Create
        interstitialAd = MediationService.Instance.CreateInterstitialAd(iAd);

        //Subscribe to events
        interstitialAd.OnLoaded += AdLoaded;
        interstitialAd.OnFailedLoad += AdFailedToLoad;

        interstitialAd.OnShowed += AdShown;
        interstitialAd.OnFailedShow += AdFailedToShow;
        interstitialAd.OnClosed += AdClosed;
        interstitialAd.OnClicked += AdClicked;

        // Impression Event
        MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
    }

    void InitializationComplete()
    {
        SetupAd();
        interstitialAd.Load();
    }

    void InitializationFailed(Exception e)
    {
        Debug.Log("Initialization Failed: " + e.Message);
    }

    // Implement load event callback methods:
    void AdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad loaded.");
        // Execute logic for when the ad has loaded
    }

    void AdFailedToLoad(object sender, LoadErrorEventArgs args)
    {
        Debug.Log("Ad failed to load.");
        // Execute logic for the ad failing to load.
    }

    // Implement show event callback methods:
    void AdShown(object sender, EventArgs args)
    {
        Debug.Log("Ad shown successfully.");
        // Execute logic for the ad showing successfully.

    }

    void AdFailedToShow(object sender, ShowErrorEventArgs args)
    {
        Debug.Log("Ad failed to show.");
        // Execute logic for the ad failing to show.
        if (currentAdType != AdType.REWARDED || interstitialAd.AdState == AdState.Unloaded)
        {
            interstitialAd.Load();
            //Returns to main menu from a game scene
            if (GameUI.instance)
                GameUI.instance.ReturnToMainMenu();
        }
    }

    private void AdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");
        // Execute logic after an ad has been closed.
        if (currentAdType == AdType.SKIPPABLE)
        {
            interstitialAd.Load();
            //Returns to main menu from a game scene
            if (GameUI.instance)
                GameUI.instance.ReturnToMainMenu();
        }
        if (currentAdType == AdType.REWARDED)
        {

        }
    }

    void AdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad has been clicked");
        // Execute logic after an ad has been clicked.
    }

    void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
    }

    public void ShowAd()
    {
        // Ensure the ad has loaded, then show it.
        if (interstitialAd.AdState == AdState.Loaded)
        {
            interstitialAd.Show();
        }
    }

    public void PlayAd(AdType type)
    {

        currentAdType = type;

        switch (currentAdType)
        {
            case AdType.BANNER:
                //Advertisement.Load(androidBanner, this);
                break;
            case AdType.SKIPPABLE:
                if (interstitialAd.AdState == AdState.Loaded)
                {
                    interstitialAd.Show();
                }
                else
                {
                    interstitialAd.Load();
                    if (GameUI.instance)
                        GameUI.instance.ReturnToMainMenu();
                }
                break;
            case AdType.REWARDED:
                //Advertisement.Load(androidRewarded, this);
                break;
            default:
                break;
        }

    }

    #region Old and likely will be deleted
    //public void OnInitializationComplete()
    //{
    //    PlayAd(AdType.BANNER);
    //    //Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    //    Debug.Log("Unity Ads initialization complete.");
    //}

    //public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    //{
    //    Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    //}

    //public void OnUnityAdsAdLoaded(string placementId)
    //{
    //    switch (currentAdType)
    //    {
    //        case AdType.BANNER:
    //            //Advertisement.Banner.Show("androidBanner");
    //            break;
    //        case AdType.SKIPPABLE:
    //            //Advertisement.Show(androidSkippable, this);
    //            break;
    //        case AdType.REWARDED:
    //            //Advertisement.Show(androidRewarded, this);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    ////public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    ////{
    ////    Debug.LogError("Ad failed to load: " + message);
    ////}

    ////public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    ////{
    ////    Debug.LogError("Ad failed to show: " + message);
    ////}

    //public void OnUnityAdsShowStart(string placementId)
    //{

    //}

    //public void OnUnityAdsShowClick(string placementId)
    //{

    //}

    //public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    //{
    //    if (currentAdType == AdType.SKIPPABLE)
    //    {
    //        //Returns to main menu from a game scene
    //        if (GameUI.instance)
    //            GameUI.instance.ReturnToMainMenu();
    //    }
    //    if (currentAdType == AdType.REWARDED)
    //    {

    //    }
    //}
    #endregion Old and likely will be deleted

}