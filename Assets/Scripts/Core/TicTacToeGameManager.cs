using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGameManager : MonoBehaviour
{
    public static TicTacToeGameManager Instance { get; private set; }

    [Header("Board")]
    [SerializeField] private BoardCell[] boardCells = new BoardCell[9];
    [SerializeField] private Image boardBackgroundImage;
    [SerializeField] private Image gridImage;

    [Header("UI")]
    [SerializeField] private HUDController hudController;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private WinStrikeAnimator winStrikeAnimator;

    private CellState[] boardStates = new CellState[9];
    private CellState currentTurn = CellState.X;

    private bool gameEnded;
    private float matchDuration;
    private int player1Moves;
    private int player2Moves;

    private readonly int[][] winPatterns =
    {
        new[] {0, 1, 2},
        new[] {3, 4, 5},
        new[] {6, 7, 8},
        new[] {0, 3, 6},
        new[] {1, 4, 7},
        new[] {2, 5, 8},
        new[] {0, 4, 8},
        new[] {2, 4, 6}
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ApplyTheme();
        RestartGame();
    }

    private void Update()
    {
        if (gameEnded)
            return;

        matchDuration += Time.deltaTime;
        hudController.UpdateTimer(matchDuration);
    }

    public void RestartGame()
    {
        gameEnded = false;
        matchDuration = 0f;
        player1Moves = 0;
        player2Moves = 0;
        currentTurn = CellState.X;

        for (int i = 0; i < boardStates.Length; i++)
        {
            boardStates[i] = CellState.Empty;
            boardCells[i].Clear();
        }

        hudController.UpdateTimer(matchDuration);
        hudController.UpdateMoves(player1Moves, player2Moves);
        hudController.UpdateTurn(currentTurn);

        gameOverPopup.Hide();

        if (winStrikeAnimator != null)
            winStrikeAnimator.Hide();
    }

    public void TryPlaceMark(int index)
    {
        if (gameEnded)
            return;

        if (index < 0 || index >= boardStates.Length)
            return;

        if (boardStates[index] != CellState.Empty)
            return;

        boardStates[index] = currentTurn;
        boardCells[index].SetMark(GetSpriteForTurn(currentTurn));

        if (currentTurn == CellState.X)
            player1Moves++;
        else
            player2Moves++;

        hudController.UpdateMoves(player1Moves, player2Moves);

        AudioManager.Instance?.PlayPlacement();

        if (CheckWin(currentTurn, out int[] winningPattern))
        {
            HandleWin(winningPattern);
            return;
        }

        if (CheckDraw())
        {
            HandleDraw();
            return;
        }

        SwitchTurn();
        hudController.UpdateTurn(currentTurn);
    }

    private void SwitchTurn()
    {
        currentTurn = currentTurn == CellState.X ? CellState.O : CellState.X;
    }

    private bool CheckWin(CellState player, out int[] winningPattern)
    {
        for (int i = 0; i < winPatterns.Length; i++)
        {
            int a = winPatterns[i][0];
            int b = winPatterns[i][1];
            int c = winPatterns[i][2];

            if (boardStates[a] == player &&
                boardStates[b] == player &&
                boardStates[c] == player)
            {
                winningPattern = winPatterns[i];
                return true;
            }
        }

        winningPattern = null;
        return false;
    }

    private bool CheckDraw()
    {
        for (int i = 0; i < boardStates.Length; i++)
        {
            if (boardStates[i] == CellState.Empty)
                return false;
        }

        return true;
    }

    private void HandleWin(int[] winningPattern)
    {
        gameEnded = true;
        DisableBoard();

        GameResult result = currentTurn == CellState.X ? GameResult.Player1Win : GameResult.Player2Win;

        StatsManager.Instance?.RecordGame(result, matchDuration);
        AudioManager.Instance?.PlayWin();

        if (winStrikeAnimator != null && winningPattern != null)
        {
            BoardCell startCell = boardCells[winningPattern[0]];
            BoardCell endCell = boardCells[winningPattern[2]];
            winStrikeAnimator.Play(startCell, endCell);
        }

        gameOverPopup.Setup(result, matchDuration);
        gameOverPopup.Show();
    }

    private void HandleDraw()
    {
        gameEnded = true;
        DisableBoard();

        StatsManager.Instance?.RecordGame(GameResult.Draw, matchDuration);

        gameOverPopup.Setup(GameResult.Draw, matchDuration);
        gameOverPopup.Show();
    }

    private void DisableBoard()
    {
        for (int i = 0; i < boardCells.Length; i++)
        {
            boardCells[i].Disable();
        }
    }

    private Sprite GetSpriteForTurn(CellState turn)
    {
        ThemeData theme = ThemeManager.Instance != null ? ThemeManager.Instance.CurrentTheme : null;

        if (theme == null)
            return null;

        return turn == CellState.X ? theme.xSprite : theme.oSprite;
    }

    public void ApplyTheme()
    {
        ThemeData theme = ThemeManager.Instance != null ? ThemeManager.Instance.CurrentTheme : null;

        if (theme == null)
            return;

        if (boardBackgroundImage != null && theme.boardBackground != null)
        {
            boardBackgroundImage.sprite = theme.boardBackground;
        }

        if (gridImage != null && theme.gridSprite != null)
        {
            gridImage.sprite = theme.gridSprite;
        }
    }

    public void OpenSettings()
    {
        AudioManager.Instance?.PlayButtonClick();
        settingsPopup.Show();
    }
}