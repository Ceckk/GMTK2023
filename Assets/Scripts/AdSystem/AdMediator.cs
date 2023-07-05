using System;

public abstract class AdMediator
{
    protected AdManager _manager;

    public abstract void Register();
    public abstract void Deregister();
    public abstract void Init();
    public abstract bool ShowRewardedAd(string placement = null);
    public abstract void LoadRewardedAd();
    public abstract bool IsRewardedAdAvailable();
    public abstract bool NeedGDPRConsent();
    public abstract void SetGDPRConsent(bool consent);

#if UNITY_IOS
    public abstract bool IsAppTrackingAvailable();
    public abstract bool HasAppTrackingConsent();
    public abstract bool HasAnsweredAppTrackingConsent();
#endif

    public AdMediator(AdManager manager)
    {
        _manager = manager;
    }
}
