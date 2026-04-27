using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private ThemeSelectionPopup themeSelectionPopup;
    [SerializeField] private StatsPopup statsPopup;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private PopupBase exitConfirmationPopup;

    public void OnPlayPressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        themeSelectionPopup.Show();
    }

    public void OnStatsPressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        statsPopup.Show();
    }

    public void OnSettingsPressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        settingsPopup.Show();
    }

    public void OnExitPressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        exitConfirmationPopup.Show();
    }

    public void OnConfirmExit()
    {
        AudioManager.Instance?.PlayButtonClick();
        Application.Quit();
    }

    public void OnCancelExit()
    {
        AudioManager.Instance?.PlayButtonClick();
        exitConfirmationPopup.Hide();
    }
}