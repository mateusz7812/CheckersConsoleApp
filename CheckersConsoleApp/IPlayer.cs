namespace CheckersConsoleApp;

public interface IPlayer
{
    public GameSide Side { get; init; }
    public Move GetMove(Board board, List<Move> availableMoves);
}