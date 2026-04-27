using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    private StatsData statsData = new StatsData();
    public StatsData CurrentStats => statsData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadStats();
    }

    public void RecordGame(GameResult result, float duration)
    {
        statsData.TotalGames++;
        statsData.TotalDuration += duration;

        switch (result)
        {
            case GameResult.Player1Win:
                statsData.Player1Wins++;
                break;
            case GameResult.Player2Win:
                statsData.Player2Wins++;
                break;
            case GameResult.Draw:
                statsData.Draws++;
                break;
        }

        SaveStats();
    }

    private void SaveStats()
    {
        PlayerPrefs.SetInt(SaveKeys.TotalGames, statsData.TotalGames);
        PlayerPrefs.SetInt(SaveKeys.Player1Wins, statsData.Player1Wins);
        PlayerPrefs.SetInt(SaveKeys.Player2Wins, statsData.Player2Wins);
        PlayerPrefs.SetInt(SaveKeys.Draws, statsData.Draws);
        PlayerPrefs.SetFloat(SaveKeys.TotalDuration, statsData.TotalDuration);
        PlayerPrefs.Save();
    }

    private void LoadStats()
    {
        statsData.TotalGames = PlayerPrefs.GetInt(SaveKeys.TotalGames, 0);
        statsData.Player1Wins = PlayerPrefs.GetInt(SaveKeys.Player1Wins, 0);
        statsData.Player2Wins = PlayerPrefs.GetInt(SaveKeys.Player2Wins, 0);
        statsData.Draws = PlayerPrefs.GetInt(SaveKeys.Draws, 0);
        statsData.TotalDuration = PlayerPrefs.GetFloat(SaveKeys.TotalDuration, 0f);
    }

    public void ResetStats()
    {
        statsData = new StatsData();
        SaveStats();
    }
}