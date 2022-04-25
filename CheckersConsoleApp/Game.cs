namespace CheckersConsoleApp;

public class Game
{
    public Board Board { get; private set; } = Board.Initial;
    public List<IPlayer> Players { get; init; }

    public Game(List<IPlayer> players)
    {
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
                Move move;
                do
                {
                    List<Move> availableMoves = Board.GetAvailableMoves(player.Side);
                    if (!availableMoves.Any())
                    {
                        win = (int) GameSide.Black + (int) GameSide.White - player.Side;
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

                    if (availableMoves.Count == 1)
                        move = availableMoves[0];
                    else
                    {
                        if (availableMoves.Any(m => m.Taking))
                        {
                            var availableTakes = availableMoves.Where(m => m.Taking);
                            var availableTakesWithRates = availableTakes.Select(take =>
                                (
                                    take,
                                    CountTakes(Board.PawnAt(take.From)!.Side, take, Board)
                                )
                            );
                            move = availableTakesWithRates.OrderByDescending(t => t.Item2).First().Item1;
                        }
                        else
                            move = player.GetMove(Board, availableMoves);
                    }

                    Board = Board.MakeMove(move);
                } while (move.Taking);
            }
        }

        foreach (var player in Players)
        {
            player.AnnounceWinner(win);
        }
    }

    private int CountTakes(GameSide side, Move move, Board board)
    {
        var boardCopy = board.Clone();
        boardCopy.MakeMove(move);
        var takes = boardCopy.GetAvailableMoves(side).Where(m => m.Taking);
        var counts = takes.Select(t => CountTakes(side, t, boardCopy)).DefaultIfEmpty(0);
        return 1 + counts.Max();
    }
}