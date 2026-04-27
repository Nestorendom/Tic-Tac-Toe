using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : PopupBase
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text durationText;

    public void Setup(GameResult result, float matchDuration)
    {
        resultText.text = GetResultLabel(result);
        durationText.text = $"Match Duration: {TimeFormatter.Format(matchDuration)}";
    }

    private string GetResultLabel(GameResult result)
    {
        switch (result)
        {
            case GameResult.Player1Win:
                return "Player 1 Wins";
            case GameResult.Player2Win:
                return "Player 2 Wins";
            case GameResult.Draw:
                return "Draw";
            default:
                return "Game Over";
        }
    }

    public void OnRetryPressed()
    {
        AudioManager.Instance?.PlayButtonClick();

        if (TicTacToeGameManager.Instance != null)
            TicTacToeGameManager.Instance.RestartGame();
    }

    public void OnExitPressed()
    {
        AudioManager.Instance?.PlayButtonClick();
        SceneManager.LoadScene("PlayScene");
    }
}