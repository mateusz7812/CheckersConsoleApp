namespace CheckersConsoleApp;

public class Game
{
    public IBoardGenerator BoardGenerator { get; init; }
    public Board Board { get; private set; } = Board.Initial;
    public List<IPlayer> Players { get; init; }

    public Game(IBoardGenerator boardGenerator, List<IPlayer> players)
    {
        BoardGenerator = boardGenerator;
        Players = players!.OrderBy(p => p.Side).ToList();
    }
    public void Run()
    {
        GameSide? win = null;
        bool draw = false;
        int queenMovesCounter = 0;
        while (win is null && draw == false) 
        {
            foreach (var player in Players)
            {
                List<Move> availableMoves = BoardGenerator.GetAvailableMoves(Board, player.Side);
                if (!availableMoves.Any())
                {
                    win = 1 - player.Side;
                    break;
                }

                if (Board.Pawns.Any(p => p.IsQueen && p.Side == GameSide.Black)
                    && Board.Pawns.Any(p => p.IsQueen && p.Side == GameSide.White))
                    queenMovesCounter++;
                if (queenMovesCounter > 15)
                {
                    win = null;
                    draw = true;
                    break;
                }
                Move move;
                Console.WriteLine(availableMoves.Count);
                if (availableMoves.Count == 1)
                    move = availableMoves[0];         
                else 
                    move = player.GetMove(Board, availableMoves);
                Board = BoardGenerator.MakeMove(Board, move);
                
            }
        }

        foreach (var player in Players)
        {
            player.AnnounceWinner(win.Value);
        }
    }
}