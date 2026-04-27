using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemeSelectionPopup : PopupBase
{
    [SerializeField] private TMP_Text selectedThemeText;
    [SerializeField] private Button[] themeButtons;
    [SerializeField] private Color selectedTextColor = new(0.7f, 0.7f, 0.7f, 1f);
    [SerializeField] private Color unselectedTextColor = Color.white;

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
        if (ThemeManager.Instance == null)
            return;

        if (ThemeManager.Instance.Themes != null && ThemeManager.Instance.Themes.Length > 0)
        {
            selectedIndex = Mathf.Clamp(selectedIndex, 0, ThemeManager.Instance.Themes.Length - 1);

            if (selectedThemeText != null)
            {
                ThemeData theme = ThemeManager.Instance.Themes[selectedIndex];
                selectedThemeText.text = $"Selected Theme: {theme.themeName}";
            }
        }

        if (themeButtons == null)
            return;

        for (int i = 0; i < themeButtons.Length; i++)
        {
            Button button = themeButtons[i];
            if (button == null)
                continue;

            bool isSelected = i == selectedIndex;
            button.interactable = !isSelected;

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
            if (buttonText != null)
                buttonText.color = isSelected ? selectedTextColor : unselectedTextColor;
        }
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
