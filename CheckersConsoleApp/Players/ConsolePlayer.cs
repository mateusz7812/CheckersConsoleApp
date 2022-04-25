namespace CheckersConsoleApp;

public class ConsolePlayer: IPlayer
{
    private static bool winnerAnnounced = false;
    public GameSide Side { get; init; }
    public Move GetMove(Board board, List<Move> availableMoves)
    {
        //Console.Clear();
        Console.WriteLine($"Player with {Side}");
        PrintBoard(board);
        Console.WriteLine("Available pawns:");
        var pawnsPositions = availableMoves.Select(m => m.From).Distinct().ToList();
        for (int i = 0; i < pawnsPositions.Count(); i++)
        {
            var pawn = pawnsPositions[i];
            Console.WriteLine($"{i}. {pawn}");
        }
        Console.Write("Choose pawn:");
        int choosePawn = Int32.Parse(Console.ReadKey().KeyChar.ToString());
        var pawnPosition = pawnsPositions[choosePawn];
        Console.WriteLine("\nAvailable moves:");
        var availableMovesForPawn = availableMoves.Where(m => m.From == pawnPosition).ToList();
        int choose;
        if (availableMovesForPawn.Count == 1)
            choose = 0;
        else
        {
            for (var index = 0; index < availableMovesForPawn.Count; index++)
            {
                var move = availableMovesForPawn[index];
                Console.WriteLine($"{index}. {move.From} -> {move.To}");
            }
            Console.Write("Choose move:");
            choose = Int32.Parse(Console.ReadKey().KeyChar.ToString());
        }
        return availableMovesForPawn[choose];
    }

    public void AnnounceWinner(GameSide? gameSide)
    {
        if (winnerAnnounced)
            return; 
        Console.Clear();
        Console.WriteLine(
            gameSide is null 
            ? "Draw" 
            : $"{gameSide} is the winner"
        );
        winnerAnnounced = true;
    }

    private void PrintBoard(Board board)
    {
        Console.Write(" ");
        Enumerable.Range(0, 8).ToList().ForEach(n => Console.Write($"  {n}"));
        Console.WriteLine();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (j == 0)
                {
                    Console.Write(7-i);
                }

                var pawn = board.PawnAt((j, 7-i));
                String fieldSymbol;
                if (pawn == null)
                {
                    fieldSymbol = ".";
                }
                else
                {
                    var sideName = pawn.Side.ToString();
                    if (!pawn.IsQueen)
                        sideName = sideName.ToLower();
                    fieldSymbol = sideName[0].ToString();
                }

                Console.Write($"  {fieldSymbol}");
            }
            Console.WriteLine();
        }
    }
}