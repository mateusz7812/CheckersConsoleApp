namespace CheckersConsoleApp;

public class Pawn
{
    public Pawn(GameSide side, (int, int) position)
    {
        Side = side;
        Position = position;
    }

    public bool IsQueen { get; set; } = false;
    public GameSide Side { get; }
    public (int, int) Position { get; set; }
}