using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeSelectionPopup : PopupBase
{
    [SerializeField] private TMP_Text selectedThemeText;

    private int selectedIndex;

    private void OnEnable()
    {
        if (ThemeManager.Instance != null)
        {
            selectedIndex = ThemeManager.Instance.SelectedThemeIndex;
            RefreshVisual();
        }
    }

    public void SelectTheme(int index)
    {
        selectedIndex = index;
        AudioManager.Instance?.PlayButtonClick();
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (ThemeManager.Instance == null || selectedThemeText == null)
            return;

        ThemeData theme = ThemeManager.Instance.Themes[selectedIndex];
        selectedThemeText.text = $"Selected Theme: {theme.themeName}";
    }

    public void OnStartPressed()
    {
        AudioManager.Instance?.PlayButtonClick();

        if (ThemeManager.Instance != null)
            ThemeManager.Instance.SelectTheme(selectedIndex);

        SceneManager.LoadScene("GameScene");
    }

    public void OnClosePressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
    }
}