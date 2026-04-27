using TMPro;
using UnityEngine;

public class StatsPopup : PopupBase
{
    [SerializeField] private TMP_Text totalGamesText;
    [SerializeField] private TMP_Text player1WinsText;
    [SerializeField] private TMP_Text player2WinsText;
    [SerializeField] private TMP_Text drawsText;
    [SerializeField] private TMP_Text averageDurationText;

    public override void Show()
    {
        base.Show();
        Refresh();
    }

    public void Refresh()
    {
        if (StatsManager.Instance == null)
            return;

        StatsData data = StatsManager.Instance.CurrentStats;

        totalGamesText.text = $"Total Games: {data.TotalGames}";
        player1WinsText.text = $"Player 1 Wins: {data.Player1Wins}";
        player2WinsText.text = $"Player 2 Wins: {data.Player2Wins}";
        drawsText.text = $"Draws: {data.Draws}";
        averageDurationText.text = $"Average Game Duration: {TimeFormatter.Format(data.AverageDuration)}";
    }

    public void OnClosePressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
    }
}