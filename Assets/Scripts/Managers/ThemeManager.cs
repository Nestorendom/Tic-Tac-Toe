using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    [SerializeField] private ThemeData[] themes;

    public int SelectedThemeIndex { get; private set; }

    public ThemeData CurrentTheme
    {
        get
        {
            if (themes == null || themes.Length == 0)
                return null;

            if (SelectedThemeIndex < 0 || SelectedThemeIndex >= themes.Length)
                SelectedThemeIndex = 0;

            return themes[SelectedThemeIndex];
        }
    }

    public ThemeData[] Themes => themes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SelectedThemeIndex = PlayerPrefs.GetInt(SaveKeys.SelectedThemeIndex, 0);

        if (themes != null && themes.Length > 0)
            SelectedThemeIndex = Mathf.Clamp(SelectedThemeIndex, 0, themes.Length - 1);
        else
            SelectedThemeIndex = 0;
    }

    private void Start()
    {
        RefreshCurrentSceneTheme();
    }

    public void SelectTheme(int index)
    {
        if (themes == null || themes.Length == 0)
            return;

        if (index < 0 || index >= themes.Length)
            return;

        SelectedThemeIndex = index;

        PlayerPrefs.SetInt(SaveKeys.SelectedThemeIndex, index);
        PlayerPrefs.Save();

        RefreshCurrentSceneTheme();
    }

    public void RefreshCurrentSceneTheme()
    {
        if (TicTacToeGameManager.Instance != null)
        {
            TicTacToeGameManager.Instance.ApplyTheme();
        }
    }
}