using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeSelectionPopup : PopupBase
{
    [Header("Theme Buttons Text")]
    [SerializeField] private TMP_Text[] themeTexts;

    [Header("Selected Color")]
    [SerializeField] private Color selectedColor = Color.gray;

    private int selectedIndex;

    private Color[] originalColors;

    private void Awake()
    {
        // Store original colors from Inspector
        originalColors = new Color[themeTexts.Length];

        for (int i = 0; i < themeTexts.Length; i++)
        {
            if (themeTexts[i] != null)
                originalColors[i] = themeTexts[i].color;
        }
    }

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
        for (int i = 0; i < themeTexts.Length; i++)
        {
            if (themeTexts[i] == null)
                continue;

            if (i == selectedIndex)
            {
                themeTexts[i].color = selectedColor;
            }
            else
            {
                // Restore original Inspector color
                themeTexts[i].color = originalColors[i];
            }
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