using System;
using System.Collections;
using UnityEngine;

public partial class AdManager : MonoSingleton<AdManager>
{
    public enum AdPlacement
    {
        Default,
        // OfflineEarning,
        // DoubleReward,
        // RefreshMarket,
        // CoinPage,
        // Pufferfish,
        // CrabHint,
        // EventDoubleDiveReward,
        // EventDailyQuest,
        // EventExtraSpin,
        // EventFreeSpin,
        // EventDoubleMissionReward,
        // BoostPage,
        // Gift
    }

    private bool _skipAds = false;
    private bool _onlyShowFakeAds = false;

    private bool _isShowingAd = false;

    private bool _init = false;
    private bool _consentSet = false;

    private AdMediator _mediator;

    private Action<bool> _callback;
    private AdPlacement _placement;

    public Action<AdPlacement> OnRewardedVideoAvailabilityRequested;
    public Action<AdPlacement> OnRewardedVideoClicked;
    public Action<AdPlacement, bool> OnRewardedVideoWatched;

    public bool SkipAds { get => _skipAds; set => _skipAds = value; }
    public bool OnlyShowFakeAds { get => _onlyShowFakeAds; set => _onlyShowFakeAds = value; }

    public bool IsShowingAd { get => _isShowingAd; }

    public bool IsInitialized { get => _init; }
    public bool IsConsentSet { get => _consentSet; }
    public bool NeedGDPRConsent { get => _mediator != null ? _mediator.NeedGDPRConsent() : false; }

    private float _failAdsCooldownDuration = 5.0f;
    Coroutine _failAdsCooldown;
    private bool _adsOnCooldown = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // TODO: setup specific mediator
        // _mediator = new AppLovinMediator(this);
        // _mediator.Register();
    }

    void OnDestroy()
    {
        _mediator?.Deregister();
    }

    public void Init()
    {
        if (!_init)
        {
            if (_mediator != null)
            {
                _mediator.Init();
            }
            else
            {
                MediatorInitialized();
            }
        }
    }

    public string AdPlacementConversion(AdPlacement placement)
    {
        switch (placement)
        {
            // case AdPlacement.OfflineEarning:
            //     return "offlineEarning";
            // case AdPlacement.DoubleReward:
            //     return "doubleReward";
            // case AdPlacement.RefreshMarket:
            //     return "refreshMarket";
            // case AdPlacement.CoinPage:
            //     return "coinPage";
            // case AdPlacement.Pufferfish:
            //     return "pufferFish";
            // case AdPlacement.CrabHint:
            //     return "crabHint";
            // case AdPlacement.EventDoubleDiveReward:
            //     return "eventDoubleDiveReward";
            // case AdPlacement.EventDailyQuest:
            //     return "eventDailyQuest";
            // case AdPlacement.EventExtraSpin:
            //     return "eventExtraSpin";
            // case AdPlacement.EventFreeSpin:
            //     return "eventFreeSpin";
            // case AdPlacement.EventDoubleMissionReward:
            //     return "eventDoubleMissionReward";
            // case AdPlacement.BoostPage:
            //     return "boostPage";
            // case AdPlacement.Gift:
            //     return "doubleGift";
            default:
                return null;
        }
    }

#if UNITY_IOS
    public bool IsAppTrackingAvailable()
    {
        return _mediator != null && _mediator.IsAppTrackingAvailable();
    }

    public bool HasAnsweredAppTrackingConsent()
    {
        return _mediator != null && _mediator.HasAnsweredAppTrackingConsent();
    }
#endif

    public void MediatorInitialized()
    {
        _init = true;
    }

    public void SetConsent(bool advertisementConsent)
    {
#if UNITY_IOS
        if (_mediator != null && _mediator.IsAppTrackingAvailable())
        {
            advertisementConsent = _mediator.HasAppTrackingConsent();
        }
#endif

        if (_mediator != null)
        {
            _mediator.SetGDPRConsent(advertisementConsent);
        }

        _consentSet = true;        
    }

    public void ShowRewardedAd(AdPlacement placement, Action<bool> callback)
    {
        OnRewardedVideoClicked?.Invoke(placement);
        _placement = placement;
        _callback = callback;
        _isShowingAd = true;

        if (SkipAds)
        {
            RewardedCallback(true);
        }
        else
        {
            if (!_onlyShowFakeAds && _mediator != null && _mediator.IsRewardedAdAvailable() && _mediator.ShowRewardedAd(AdPlacementConversion(placement)))
            {
                AudioListener.pause = true;
            }
            else if (_adsOnCooldown == true)
            {
                // TODO: show ad in cooldown
            }
            else
            {
                // TODO: show falke ad
            }
        }
    }

    public bool IsRewardedAdAvailable(AdPlacement placement)
    {
        OnRewardedVideoAvailabilityRequested?.Invoke(placement);
        return true;
    }

    public void RewardedCallback(bool success)
    {
        Debug.Log("RewardedCallback success: " + success);
        _isShowingAd = false;
        _callback?.Invoke(success);
        OnRewardedVideoWatched?.Invoke(_placement, success);
        if (success == false)
        {
            if (_failAdsCooldown != null) StopCoroutine(FailAdsCooldown());
            _failAdsCooldown = StartCoroutine(FailAdsCooldown());
        }

        LoadRewardedAd();
        AudioListener.pause = false;
    }

    IEnumerator FailAdsCooldown()
    {
        _adsOnCooldown = true;
        yield return new WaitForSecondsRealtime(_failAdsCooldownDuration);
        _adsOnCooldown = false;
    }

    public void LoadRewardedAd()
    {
        if (!_onlyShowFakeAds && _mediator != null)
        {
            _mediator.LoadRewardedAd();
        }
    }
}
