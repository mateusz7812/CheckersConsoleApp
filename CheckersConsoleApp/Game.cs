namespace CheckersConsoleApp;

public class Game
{
    public IBoardGenerator BoardGenerator { get; init; }
    public Board Board { get; private set; } = Board.Initial;
    public List<IPlayer> Players { get; init; }

    public void Run()
    {
        while (true)
        {
            foreach (var player in Players)
            {
                var availableMoves = BoardGenerator.GetAvailableMoves(Board, player.Side);
                var move = player.GetMove(Board, availableMoves);
                Board = BoardGenerator.MakeMove(Board, move);
            }
        }
    }
}