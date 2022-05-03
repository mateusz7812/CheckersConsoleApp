namespace CheckersConsoleApp;

public class Pawn
{
    public Pawn(GameSide side, (int, int) position, bool isQueen = false)
    {
        Side = side;
        Position = position;
        IsQueen = isQueen;
    }

    public bool IsQueen { get; set; }
    public GameSide Side { get; }
    public (int, int) Position { get; set; }

    public Pawn Clone()
    {
        return new Pawn(Side, (Position.Item1, Position.Item2), IsQueen);
    }

    public override string ToString()
    {
        return $"{Side} {Position} {IsQueen}";
    }  
}