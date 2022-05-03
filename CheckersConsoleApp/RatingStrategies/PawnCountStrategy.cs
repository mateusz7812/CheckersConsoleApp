namespace CheckersConsoleApp.RatingStrategies;

public class PawnCountStrategy: IRatingStrategy
{
    public int Rate(Board board, GameSide side)
    {
        return board.Pawns.Count(p => p.Side == side) / board.Pawns.Count(p => p.Side != side);
    }
}