using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : PopupBase
{
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;

    private bool isRefreshing;
    private bool listenersRegistered;

    private void Awake()
    {
        RegisterListeners();
    }

    private void OnEnable()
    {
        RegisterListeners();
        Refresh();
    }

    private void OnDisable()
    {
        UnregisterListeners();
    }

    public void Refresh()
    {
        if (AudioManager.Instance == null || bgmToggle == null || sfxToggle == null)
            return;

        isRefreshing = true;
        bgmToggle.SetIsOnWithoutNotify(AudioManager.Instance.BgmEnabled);
        sfxToggle.SetIsOnWithoutNotify(AudioManager.Instance.SfxEnabled);
        isRefreshing = false;
    }

    public void OnBgmToggleChanged(bool isOn)
    {
        if (isRefreshing)
            return;

        AudioManager.Instance?.SetBgmEnabled(isOn);
    }

    public void OnSfxToggleChanged(bool isOn)
    {
        if (isRefreshing)
            return;

        AudioManager.Instance?.SetSfxEnabled(isOn);
    }

    public void OnClosePressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
    }

    private void RegisterListeners()
    {
        if (listenersRegistered || bgmToggle == null || sfxToggle == null)
            return;

        bgmToggle.onValueChanged.AddListener(OnBgmToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
        listenersRegistered = true;
    }

    private void UnregisterListeners()
    {
        if (!listenersRegistered || bgmToggle == null || sfxToggle == null)
            return;

        bgmToggle.onValueChanged.RemoveListener(OnBgmToggleChanged);
        sfxToggle.onValueChanged.RemoveListener(OnSfxToggleChanged);
        listenersRegistered = false;
    }
}
