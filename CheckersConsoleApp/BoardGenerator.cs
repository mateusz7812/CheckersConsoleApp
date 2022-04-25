namespace CheckersConsoleApp;

public class BoardGenerator: IBoardGenerator
{
    public List<Move> GetAvailableMoves(Board board, GameSide side)
    {
        var availableMoves = board.Pawns
            .FindAll(p => p.Side == side)
            .SelectMany(board.GetMovesForPawn)
            .ToList();
        
        if (availableMoves.Any(m => m.Taking))
            return availableMoves.Where(m => m.Taking).ToList();
        return availableMoves;
    }

    public Board MakeMove(Board board, Move move)
    {
        board.MakeMove(move);
        var positionsBetween = Board.PositionsBetween(move);
        foreach (var position in positionsBetween)
        {
            board.TakeIfHas(position);
        }
        return board;
    }
}