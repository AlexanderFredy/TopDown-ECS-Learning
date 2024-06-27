using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;

    [SerializeField] RewardedAdsButton _rewButton;

    private string _gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
    #if UNITY_IOS
                _gameId = _iOSGameId;
    #elif UNITY_ANDROID
                _gameId = _androidGameId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {          
            Advertisement.Initialize(_gameId, _testMode, this);
            Invoke("LoadReward", 5f);
        }
    }

    private void LoadReward()
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.AddListener(_rewButton);
            _rewButton.LoadAd();
        }
        else
            Debug.Log("Advertisement is not loaded");
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }
}