using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CheatManager : MonoSingleton<CheatManager>
{
    [SerializeField] private GameObject _cheatPanel;
    [SerializeField] private Button _cheatButton;
    [SerializeField] private GameObject _cheatButtonIcon;
    [SerializeField] private Button _overlayBGButton;

    [Space]
    [SerializeField] private Button _openConsoleButton;

    [Space]
    [SerializeField] private Button _deleteLocalAndOnlineSaveButton;
    [SerializeField] private Button _deleteLocalSaveButton;
    [SerializeField] private Button _deleteOnlineSaveButton;

    [Space]
    [SerializeField] private Button _skipTutorialButton;
    [SerializeField] private Button _unlockAll;
    [SerializeField] private Button _adsDebuggerButton;
    [SerializeField] private Button _unloadUnusedAssets;

    [Space]
    [SerializeField] private TMP_InputField _simulateOfflineTimeInputAmount;
    [SerializeField] private Button _simulateOfflineTimeButton;
    [SerializeField] private TMP_InputField _fastforwardTimeInputAmount;
    [SerializeField] private Button _fastforwardTimeButton;

    [Space]
    [SerializeField] private Button _resetResolutionButton;
    [SerializeField] private TextMeshProUGUI _resolutionValue;
    [SerializeField] private Slider _resolutionSlider;
    [SerializeField] private Button _resetRenderScaleButton;
    [SerializeField] private TextMeshProUGUI _renderScaleValue;
    [SerializeField] private Slider _renderScaleSlider;

    [Space]
    [SerializeField] private AnimationCurve _timeScaleCurve;
    [SerializeField] private Button _resetTimeScaleButton;
    [SerializeField] private TextMeshProUGUI _timeScaleValue;
    [SerializeField] private Slider _timeScaleSlider;

    [Space]
    [SerializeField] private Toggle _cheatButtonVisibleToggle;
    [SerializeField] private Toggle _skipAdsToggle;
    [SerializeField] private Toggle _skipIAPToggle;
    [SerializeField] private Toggle _onlyShowFakeAdsToggle;

    [Space]
    [SerializeField] private UniversalRenderPipelineAsset _renderPipelineAsset;
    [SerializeField] private float _defaultRenderScale = 1;

    [Space]
    [SerializeField] private bool _defaultCheatButtonVisible = true;
    [SerializeField] private bool _defaulSkipAdsEnabled = false;
    [SerializeField] private bool _defaulSkipIAPEnabled = false;
    [SerializeField] private bool _defaulOnlyShowFakeAdsEnabled = false;

    private Vector2 _defaultResolution = Vector2.zero;
    private List<string> _currencyIds = new List<string>();
    private bool _init = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (_defaultResolution == Vector2.zero)
            _defaultResolution = new Vector2(Display.main.systemWidth, Display.main.systemHeight);

        //if non-development build turn itself off
        if (!Debug.isDebugBuild)
            gameObject.SetActive(false);

        _simulateOfflineTimeInputAmount.text = "10800";
        _fastforwardTimeInputAmount.text = "60";

        _cheatButton.onClick.AddListener(OpenCheatPanel);
        _overlayBGButton.onClick.AddListener(CloseCheatPanel);

        _openConsoleButton.onClick.AddListener(OpenConsole);
        _openConsoleButton.onClick.AddListener(CloseCheatPanel);

        _deleteLocalAndOnlineSaveButton.onClick.AddListener(DeleteLocalAndOnlineSave);
        _deleteLocalSaveButton.onClick.AddListener(DeleteLocalSave);

        _skipTutorialButton.onClick.AddListener(SkipTutorial);
        _skipTutorialButton.onClick.AddListener(CloseCheatPanel);

        _unloadUnusedAssets.onClick.AddListener(UnloadUnusedAssets);
        _unloadUnusedAssets.onClick.AddListener(CloseCheatPanel);

        _unlockAll.onClick.AddListener(UnlockAll);
        _unlockAll.onClick.AddListener(CloseCheatPanel);

        _adsDebuggerButton.onClick.AddListener(OpenAdsDebugger);
        _adsDebuggerButton.onClick.AddListener(CloseCheatPanel);

        _simulateOfflineTimeButton.onClick.AddListener(SimulateOfflineTime);
        _simulateOfflineTimeButton.onClick.AddListener(CloseCheatPanel);

        _fastforwardTimeButton.onClick.AddListener(FastforwardTime);
        _fastforwardTimeButton.onClick.AddListener(CloseCheatPanel);

        _resetResolutionButton.onClick.AddListener(ResetResolution);
        _resetResolutionButton.onClick.AddListener(CloseCheatPanel);
        _resolutionSlider.onValueChanged.AddListener(OnResolutionSliderValueChange);

        _resetRenderScaleButton.onClick.AddListener(ResetRenderScale);
        _resetRenderScaleButton.onClick.AddListener(CloseCheatPanel);
        _renderScaleSlider.onValueChanged.AddListener(OnRenderScaleSliderValueChange);

        _resetTimeScaleButton.onClick.AddListener(ResetTimeScale);
        _resetTimeScaleButton.onClick.AddListener(CloseCheatPanel);
        _timeScaleSlider.onValueChanged.AddListener(OnTimeScaleSliderValueChange);

        _cheatButtonVisibleToggle.onValueChanged.AddListener(EnableCheatButtonVisibility);
        _skipAdsToggle.onValueChanged.AddListener(EnableSkipAds);
        _skipIAPToggle.onValueChanged.AddListener(EnableSkipIAP);
        _onlyShowFakeAdsToggle.onValueChanged.AddListener(EnableOnlyShowFakeAds);

        Init();
    }

    void OnDestroy()
    {
        _cheatButton.onClick.RemoveAllListeners();
        _overlayBGButton.onClick.RemoveAllListeners();

        _deleteLocalAndOnlineSaveButton.onClick.RemoveAllListeners();
        _deleteLocalSaveButton.onClick.RemoveAllListeners();

        _skipTutorialButton.onClick.RemoveAllListeners();
        _unlockAll.onClick.RemoveAllListeners();
        _adsDebuggerButton.onClick.RemoveAllListeners();
        _unloadUnusedAssets.onClick.RemoveAllListeners();

        _simulateOfflineTimeButton.onClick.RemoveAllListeners();
        _fastforwardTimeButton.onClick.RemoveAllListeners();

        _resetTimeScaleButton.onClick.RemoveAllListeners();
        _timeScaleSlider.onValueChanged.RemoveAllListeners();

        _cheatButtonVisibleToggle.onValueChanged.RemoveAllListeners();
        _skipAdsToggle.onValueChanged.RemoveAllListeners();
        _skipIAPToggle.onValueChanged.RemoveAllListeners();
        _onlyShowFakeAdsToggle.onValueChanged.RemoveAllListeners();
    }

    public void Init()
    {
        EnableCheatButtonVisibility(_defaultCheatButtonVisible);
        EnableSkipAds(_defaulSkipAdsEnabled);
        EnableSkipIAP(_defaulSkipIAPEnabled);
        EnableOnlyShowFakeAds(_defaulOnlyShowFakeAdsEnabled);

        SetupResolutionSlider();
        ResetRenderScale();

        ResetTimeScale();
    }

    private void OpenCheatPanel()
    {
        _cheatPanel.SetActive(true);
    }

    private void CloseCheatPanel()
    {
        _cheatPanel.SetActive(false);
    }

    private void EnableCheatButtonVisibility(bool enabled)
    {
        _cheatButtonIcon.SetActive(enabled);
    }

    private void OpenConsole()
    {
        // SRDebug.Instance.ShowDebugPanel(false);
    }

    private void DeleteLocalAndOnlineSave()
    {
        SaveManager.Instance.Delete();
    }

    private void DeleteOnlineSave()
    {
        SaveManager.Instance.DeleteCloud((success) =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    private void DeleteLocalSave()
    {
        SaveManager.Instance.DeleteLocal();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetupResolutionSlider()
    {
        _resolutionSlider.SetValueWithoutNotify(DeviceResolutionManager.Instance.ResolutionMultiplier);
        _resolutionValue.text = string.Format("Current Resolution: {0}x{1}", Screen.width, Screen.height);
    }

    private void ResetResolution()
    {
        _resolutionSlider.SetValueWithoutNotify(DeviceResolutionManager.Instance.ResolutionMultiplier);
        SetResolutionMultiplier(DeviceResolutionManager.Instance.ResolutionMultiplier);
    }

    private void OnResolutionSliderValueChange(float value)
    {
        var finalValue = Mathf.Lerp(0.1f, 1, value);
        SetResolutionMultiplier(finalValue);
    }

    private void SetResolutionMultiplier(float value)
    {
        var resolutionWidth = (int)(_defaultResolution.x * value);
        var resolutionHeight = (int)(_defaultResolution.y * value);
        Screen.SetResolution(resolutionWidth, resolutionHeight, true);
        _resolutionValue.text = string.Format("Current Resolution: {0}x{1}", resolutionWidth, resolutionHeight);
    }

    private void ResetRenderScale()
    {
        var renderScale = _defaultRenderScale;
        _renderScaleSlider.SetValueWithoutNotify(renderScale);
        SetRenderScale(renderScale);
    }

    private void OnRenderScaleSliderValueChange(float value)
    {
        var finalValue = Mathf.Lerp(0.1f, 1, value);
        SetRenderScale(finalValue);
    }

    private void SetRenderScale(float amount)
    {
        if (_renderPipelineAsset != null)
        {
            _renderPipelineAsset.renderScale = amount;
            GraphicsSettings.renderPipelineAsset = _renderPipelineAsset;
        }
        else
        {
            Debug.LogWarning("Lightweight Render Pipeline not set");
        }

        _renderScaleValue.text = string.Format("Current Render Scale: {0:f2} ", amount);
    }

    private void ResetTimeScale()
    {
        _timeScaleSlider.SetValueWithoutNotify(0.5f);
        SetTimeScale(1);
    }

    private void OnTimeScaleSliderValueChange(float value)
    {
        var finalValue = _timeScaleCurve.Evaluate(value);
        SetTimeScale(finalValue);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        _timeScaleValue.text = string.Format("Current Time Scale: {0:f2}", Time.timeScale);
    }

    private void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }

    private void SkipTutorial()
    {

    }

    private void UnlockAll()
    {

    }

    private void OpenAdsDebugger()
    {
        // MaxSdk.ShowMediationDebugger();
    }

    private void ShowRewardedAd()
    {
        AdManager.Instance?.ShowRewardedAd(AdManager.AdPlacement.Default, (success) => Debug.Log(success));
    }

    private void SimulateOfflineTime()
    {
        // OfflineRewardManager.Instance.CheckForReward(int.Parse(_simulateOfflineTimeInputAmount.text));
    }

    private void FastforwardTime()
    {
        TimeUtility.timeOffset += int.Parse(_fastforwardTimeInputAmount.text);
    }

    private void EnableSkipAds(bool enabled)
    {
        _skipAdsToggle.SetIsOnWithoutNotify(enabled);

        if (AdManager.Instance != null)
        {
            AdManager.Instance.SkipAds = enabled;
        }
    }

    private void EnableSkipIAP(bool enabled)
    {
        _skipIAPToggle.SetIsOnWithoutNotify(enabled);

        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.SkipIAP = enabled;
        }
    }

    private void EnableOnlyShowFakeAds(bool enabled)
    {
        _onlyShowFakeAdsToggle.SetIsOnWithoutNotify(enabled);

        if (AdManager.Instance != null)
        {
            AdManager.Instance.OnlyShowFakeAds = enabled;
        }
    }
}
