using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public enum AdType
{
    BANNER,
    SKIPPABLE,
    REWARDED
}

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance;

    public string androidID = "4542053", iOSID = "4542052";
    public string androidRewarded = "Rewarded_Android", androidSkippable = "Interstitial_Android", androidBanner = "Banner_Android";
    public string iOSRewarded = "Rewarded_iOS", iOSSkippable = "Interstitial_iOS", iOSBanner = "Banner_iOS";


    bool testMode = true;
    AdType currentAdType = AdType.BANNER;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(androidID, testMode, this);
#elif UNITY_IOS
        Advertisement.Initialize(iOSID, testMode, this);
#endif
    }

    public void OnInitializationComplete()
    {
        PlayAd(AdType.BANNER);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    public void PlayAd(AdType type)
    {

        currentAdType = type;

#if UNITY_ANDROID
        switch (currentAdType)
        {
            case AdType.BANNER:
                Advertisement.Load(androidBanner, this);
                break;
            case AdType.SKIPPABLE:
                Advertisement.Load(androidSkippable, this);
                break;
            case AdType.REWARDED:
                Advertisement.Load(androidRewarded, this);
                break;
            default:
                break;
        }
#elif UNITY_IOS

        switch (currentAdType)
        {
            case AdType.BANNER:
                Advertisement.Load(iOSBanner, this);
                break;
            case AdType.SKIPPABLE:
                Advertisement.Load(iOSSkippable, this);
                break;
            case AdType.REWARDED:
                Advertisement.Load(iOSRewarded, this);
                break;
            default:
                break;
        }
#endif
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
#if UNITY_ANDROID
        switch (currentAdType)
        {
            case AdType.BANNER:
                Advertisement.Banner.Show("androidBanner");
                break;
            case AdType.SKIPPABLE:
                Advertisement.Show(androidSkippable, this);
                break;
            case AdType.REWARDED:
                Advertisement.Show(androidRewarded, this);
                break;
            default:
                break;
        }
#elif UNITY_IOS

        switch (currentAdType)
        {
            case AdType.BANNER:
                Advertisement.Banner.Show("iOSBanner");
                break;
            case AdType.SKIPPABLE:
                Advertisement.Show(iOSSkippable, this);
                break;
            case AdType.REWARDED:
                Advertisement.Show(iOSRewarded, this);
                break;
            default:
                break;
        }
#endif
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("Ad failed to load: " + message);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError("Ad failed to show: " + message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (currentAdType == AdType.SKIPPABLE)
        {
            //Returns to main menu from a game scene
            if (GameUI.instance)
                GameUI.instance.ReturnToMainMenu();
        }
        if (currentAdType == AdType.REWARDED)
        {
            
        }
    }


}