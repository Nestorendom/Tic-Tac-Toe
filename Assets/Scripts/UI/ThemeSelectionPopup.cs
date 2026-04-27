using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemeSelectionPopup : PopupBase
{
    [SerializeField] private TMP_Text selectedThemeText;
    [SerializeField] private TMP_Text[] themeOptionTexts;
    [SerializeField] private Color unselectedTextColor = Color.white;
    [SerializeField] private Color selectedTextColor = new(0.6f, 0.6f, 0.6f, 1f);

    private int selectedIndex;
    private bool hasInitializedThemeOptionTexts;

    private void OnEnable()
    {
        InitializeThemeOptionTexts();

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

        for (int i = 0; i < themeOptionTexts.Length; i++)
        {
            if (themeOptionTexts[i] == null)
                continue;

            themeOptionTexts[i].color = i == selectedIndex ? selectedTextColor : unselectedTextColor;
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

    private void InitializeThemeOptionTexts()
    {
        if (hasInitializedThemeOptionTexts)
            return;

        hasInitializedThemeOptionTexts = true;

        if (themeOptionTexts != null && themeOptionTexts.Length > 0)
            return;

        Button[] buttons = GetComponentsInChildren<Button>(true);
        int themeButtonCount = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (IsThemeSelectionButton(buttons[i]))
                themeButtonCount++;
        }

        themeOptionTexts = new TMP_Text[themeButtonCount];
        int textIndex = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (!IsThemeSelectionButton(buttons[i]))
                continue;

            TMP_Text optionText = buttons[i].GetComponentInChildren<TMP_Text>(true);
            if (optionText == selectedThemeText)
                continue;

            themeOptionTexts[textIndex] = optionText;
            textIndex++;
        }
    }

    private bool IsThemeSelectionButton(Button button)
    {
        if (button == null)
            return false;

        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            if (button.onClick.GetPersistentTarget(i) == this &&
                button.onClick.GetPersistentMethodName(i) == nameof(SelectTheme))
            {
                return true;
            }
        }

        return false;
    }
}
