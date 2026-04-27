using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemeSelectionPopup : PopupBase
{
    [SerializeField] private TMP_Text selectedThemeText;
    [SerializeField] private TMP_Text[] themeOptionTexts;
    [SerializeField] private Color unselectedTextColor = Color.white;
    [SerializeField] private Color selectedTextColor = new(1f, 0.92f, 0.35f, 1f);

    private int selectedIndex;
    private bool hasInitializedThemeOptionTexts;
    private string[] themeOptionBaseLabels;

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

            bool isSelected = i == selectedIndex;
            themeOptionTexts[i].color = isSelected ? selectedTextColor : unselectedTextColor;
            themeOptionTexts[i].fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;

            string label = GetOptionBaseLabel(i, themeOptionTexts[i]);
            themeOptionTexts[i].text = isSelected ? $"✓ {label}" : label;
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
        {
            CacheThemeOptionBaseLabels();
            return;
        }

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

        if (themeOptionTexts.Length == 0)
            AutoWireThemeOptionsFromThemeNames();

        CacheThemeOptionBaseLabels();
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

    private void AutoWireThemeOptionsFromThemeNames()
    {
        if (ThemeManager.Instance == null || ThemeManager.Instance.Themes == null)
            return;

        ThemeData[] themes = ThemeManager.Instance.Themes;
        themeOptionTexts = new TMP_Text[themes.Length];

        TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true);
        for (int i = 0; i < themes.Length; i++)
        {
            for (int j = 0; j < allTexts.Length; j++)
            {
                if (allTexts[j] == null || allTexts[j] == selectedThemeText)
                    continue;

                if (allTexts[j].text == themes[i].themeName)
                {
                    themeOptionTexts[i] = allTexts[j];
                    break;
                }
            }
        }
    }

    private void CacheThemeOptionBaseLabels()
    {
        themeOptionBaseLabels = new string[themeOptionTexts.Length];
        for (int i = 0; i < themeOptionTexts.Length; i++)
        {
            if (themeOptionTexts[i] == null)
                continue;

            themeOptionBaseLabels[i] = themeOptionTexts[i].text.Replace("✓ ", string.Empty);
        }
    }

    private string GetOptionBaseLabel(int index, TMP_Text optionText)
    {
        if (themeOptionBaseLabels != null &&
            index >= 0 &&
            index < themeOptionBaseLabels.Length &&
            !string.IsNullOrWhiteSpace(themeOptionBaseLabels[index]))
        {
            return themeOptionBaseLabels[index];
        }

        return optionText.text.Replace("✓ ", string.Empty);
    }
}
