using System.Diagnostics;
using CheckersConsoleApp.Moves;
using CheckersConsoleApp.RatingStrategies;

namespace CheckersConsoleApp.Players;

public class MinMaxBot: Bot
{
    Stopwatch _stopwatch = new Stopwatch();
    private List<long> _timeList = new List<long>();
    private List<int> _visitedNodes = new List<int>();
    public int Level { get; }

    public override Move GetMove(Board board, List<Move> availableMoves)
    {
        _visitedNodes.Add(-5);
        _stopwatch.Restart();
        var minMaxMove = GetMinMaxMove(board, availableMoves);
        _stopwatch.Stop();
        _timeList.Add(_stopwatch.ElapsedMilliseconds);
        Console.WriteLine($"Time {_stopwatch.ElapsedMilliseconds}");
        Console.WriteLine($"Visited nodes: {_visitedNodes[^1]}");
            return minMaxMove;
    }

    public override void AnnounceWinner(GameSide? gameSide, Board board)
    {
        Console.WriteLine($"Winner: {gameSide}");
        Console.WriteLine($"Avg time: {_timeList.Average()}");
    }

    private MinMaxMove GetMinMaxMove(Board board, List<Move> availableMoves, int level = 0)
    {
        _visitedNodes[^1]++;
        var minMaxMove = availableMoves.Select(m =>
        {
            var nextBoard = board.Clone();
            var pawn = nextBoard.MakeMove(m);
            var gameSide = pawn.Side;
            var moves = nextBoard.GetAvailableMoves(1 - gameSide).ToList();
            return new MinMaxMove(m, 
                level >= Level || !moves.Any() ? 
                    RatingStrategy.Rate(nextBoard, Side) : 
                    GetMinMaxMove(nextBoard, moves, level + 1).Rating
            );
        }).MaxBy(m => m.Rating)!;
        return minMaxMove;
    } 

    public MinMaxBot(IRatingStrategy ratingStrategy, GameSide side, int level) 
        : base(ratingStrategy, side)
    {
        Level = level;
    }
    
    
}