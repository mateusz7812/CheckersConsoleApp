using CheckersConsoleApp.Moves;

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
        var pawnPosition = pawnsPositions[
            GetChoice(pawnsPositions.Select(p => $"{p}").ToArray(), "Choose pawn:")
        ];
        Console.WriteLine("\nAvailable moves:");
        var availableMovesForPawn = availableMoves.Where(m => m.From == pawnPosition).ToList();
        return availableMovesForPawn[
            GetChoice(availableMovesForPawn.Select(m => $"{m.From} => {m.To}").ToArray(), "Choose move:")
        ];
    }

    public void AnnounceWinner(GameSide? gameSide, Board board)
    {
        if (winnerAnnounced)
            return; 
        //Console.Clear();
        Console.WriteLine(
            gameSide is null 
            ? "Draw" 
            : $"{gameSide} is the winner"
        );
        PrintBoard(board);
        winnerAnnounced = true;
    }

    private void PrintBoard(Board board)
    {
        Console.WriteLine(board);
    }

    private int GetChoice(string[] options, string request)
    {
        int choice = 0;
        ConsoleKeyInfo consoleKeyInfo;
        bool tryParse = true;
        
        while(true){
            for (var index = 0; index < options.Length; index++)
            {
                Console.WriteLine($"{index}. {options[index]}");
            }

            Console.Write(request);
            consoleKeyInfo = Console.ReadKey();
            Console.WriteLine();
            tryParse = Int32.TryParse(consoleKeyInfo.KeyChar.ToString(), out choice);
            if (!tryParse
                || choice < 0
                || choice >= options.Length)
                Console.WriteLine("Wrong index");
            else
                break;
        }
        return choice;
    }
}