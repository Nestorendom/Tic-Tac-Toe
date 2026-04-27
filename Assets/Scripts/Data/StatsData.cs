[System.Serializable]
public class StatsData
{
    public int TotalGames;
    public int Player1Wins;
    public int Player2Wins;
    public int Draws;
    public float TotalDuration;

    public float AverageDuration
    {
        get
        {
            return TotalGames > 0 ? TotalDuration / TotalGames : 0f;
        }
    }
}