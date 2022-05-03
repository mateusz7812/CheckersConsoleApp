namespace CheckersConsoleApp.RatingStrategies;

public class CirclesStrategy: IRatingStrategy
{
    public int Rate(Board board, GameSide side)
    {
        return board.Pawns.Where(p => p.Side == side).Select(RateCircle).DefaultIfEmpty(0).Sum() /
               board.Pawns.Where(p => p.Side != side).Select(RateCircle).DefaultIfEmpty(1).Sum();
    }

    private int RateCircle(Pawn pawn)
    {
        var x = pawn.Position.Item1;
        var y = pawn.Position.Item2;
        if (x is > 2 and < 5 &&
            y is > 2 and < 5)
        {
            return 6;
        }
        if (x is > 0 and < 7 &&
            y is > 0 and < 7)
        {
            return 3;
        }
        return 1;
    }
}