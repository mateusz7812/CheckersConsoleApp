namespace CheckersConsoleApp;

public interface IBoardGenerator
{
    public List<Move> GetAvailableMoves(Board board, GameSide side);
    public Board MakeMove(Board board, Move move);
}