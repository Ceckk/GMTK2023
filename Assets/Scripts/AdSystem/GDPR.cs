using UnityEngine;

public static class GDPR
{
    public const string SHOWN = "SHOWN";
    public const string ADVERTISEMENT_CONSENT = "ADVERTISEMENT_CONSENT";
    public const string ANALYTICS_CONSENT = "ANALYTICS_CONSENT";

    public static bool AnalyticsConsent
    {
        get
        {
            return PlayerPrefs.HasKey(ANALYTICS_CONSENT) && PlayerPrefs.GetInt(ANALYTICS_CONSENT) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(ANALYTICS_CONSENT, value ? 1 : 0);
        }
    }

    public static bool AdvertisementConsent
    {
        get
        {
            return PlayerPrefs.HasKey(ADVERTISEMENT_CONSENT) && PlayerPrefs.GetInt(ADVERTISEMENT_CONSENT) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(ADVERTISEMENT_CONSENT, value ? 1 : 0);
        }
    }

    public static bool Shown
    {
        get
        {
            return PlayerPrefs.HasKey(SHOWN) && PlayerPrefs.GetInt(SHOWN) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(SHOWN, value ? 1 : 0);
        }
    }

    private static void Accept()
    {
        AnalyticsConsent = true;
        AdvertisementConsent = true;
    }

    private static void Refuse()
    {
        AnalyticsConsent = false;
        AdvertisementConsent = false;
    }
}
