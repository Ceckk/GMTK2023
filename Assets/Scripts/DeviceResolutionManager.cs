using UnityEngine;

public class DeviceResolutionManager : MonoSingleton<DeviceResolutionManager>
{
    private const int LOW_END_THRESHOLD = 1024;
    private const int MID_END_THRESHOLD = 2048;

    // private const float LOW_END_PERCENTAGE = 0.4f;
    // private const float MID_END_PERCENTAGE = 0.5f;
    private const float HIGH_END_PERCENTAGE = 0.6f;

    private Vector2 _defaultResolution;
    private float _resolutionMultiplier = HIGH_END_PERCENTAGE;
    private float _dpi;
    private Rect _safeArea;

    public Vector2 DefaultResolution { get => _defaultResolution;  }
    public float ResolutionMultiplier { get => _resolutionMultiplier; }
    public float Dpi { get => _dpi; }
    public Rect SafeArea { get => _safeArea; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        _defaultResolution = new Vector2(Display.main.systemWidth, Display.main.systemHeight);
        _dpi = Screen.dpi;
        _safeArea = Screen.safeArea;
    }

    public void Init()
    {
        int totalMemory = SystemInfo.systemMemorySize;

        _resolutionMultiplier = HIGH_END_PERCENTAGE;

// #if UNITY_ANDROID && !UNITY_EDITOR
        // if (totalMemory <= LOW_END_THRESHOLD)
        // {
        //     _resolutionMultiplier = LOW_END_PERCENTAGE;
        // }
        // else if (totalMemory <= MID_END_THRESHOLD)
        // {
        //     _resolutionMultiplier = MID_END_PERCENTAGE;
        // }
        // else
        // {
        //     _resolutionMultiplier = HIGH_END_PERCENTAGE;
        // }
// #endif
        var resolutionWidth = (int)(_defaultResolution.x * _resolutionMultiplier);
        var resolutionHeight = (int)(_defaultResolution.y * _resolutionMultiplier);
        Screen.SetResolution(resolutionWidth, resolutionHeight, true);
    }
}