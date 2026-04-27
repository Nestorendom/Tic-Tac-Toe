using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text player1MovesText;
    [SerializeField] private TMP_Text player2MovesText;
    [SerializeField] private TMP_Text turnText;

    public void UpdateTimer(float time)
    {
        timerText.text = $"Time: {TimeFormatter.Format(time)}";
    }

    public void UpdateMoves(int p1Moves, int p2Moves)
    {
        player1MovesText.text = $"P1 Moves: {p1Moves}";
        player2MovesText.text = $"P2 Moves: {p2Moves}";
    }

    public void UpdateTurn(CellState currentTurn)
    {
        turnText.text = currentTurn == CellState.X ? "Turn: Player 1" : "Turn: Player 2";
    }
}