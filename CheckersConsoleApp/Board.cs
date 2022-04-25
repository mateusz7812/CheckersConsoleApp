namespace CheckersConsoleApp;

public class Board
{
    public Pawn[][] Placing { get; }

    public static Board Initial = new Board(new[]
    {
        new[] {Pawn.Empty, Pawn.Black, Pawn.Empty, Pawn.Black, Pawn.Empty, Pawn.Black, Pawn.Empty, Pawn.Black},
        new[] {Pawn.Black, Pawn.Empty, Pawn.Black, Pawn.Empty, Pawn.Black, Pawn.Empty, Pawn.Black, Pawn.Empty},
        new[] {Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty},
        new[] {Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty},
        new[] {Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty},
        new[] {Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty, Pawn.Empty},
        new[] {Pawn.Empty, Pawn.White, Pawn.Empty, Pawn.White, Pawn.Empty, Pawn.White, Pawn.Empty, Pawn.White},
        new[] {Pawn.White, Pawn.Empty, Pawn.White, Pawn.Empty, Pawn.White, Pawn.Empty, Pawn.White, Pawn.Empty}
    });

    public Board(Pawn[][] placing)
    {
        this.Placing = placing;
    }
}