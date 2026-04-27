using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : PopupBase
{
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;

    private bool isRefreshing;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (AudioManager.Instance == null)
            return;

        isRefreshing = true;
        bgmToggle.isOn = AudioManager.Instance.BgmEnabled;
        sfxToggle.isOn = AudioManager.Instance.SfxEnabled;
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
}