namespace CheckersConsoleApp;

public class Board
{
    public Board(List<Pawn> pawns, List<Pawn>? taken = null)
    {
        Pawns = pawns;
        Taken = taken ?? new List<Pawn>();
    }

    public List<Pawn> Pawns { get; }
    public List<Pawn> Taken { get; }

    public static Board Initial = new (new List<Pawn>()
    {
        new (GameSide.Black, (1, 7)), new (GameSide.Black, (3, 7)), new (GameSide.Black, (5, 7)), new (GameSide.Black, (7, 7)), 
        new (GameSide.Black, (0, 6)), new (GameSide.Black, (2, 6)), new (GameSide.Black, (4, 6)), new (GameSide.Black, (6, 6)),
        new (GameSide.White, (0, 0)), new (GameSide.White, (2, 0)), new (GameSide.White, (4, 0)), new (GameSide.White, (6, 0)), 
        new (GameSide.White, (1, 1)), new (GameSide.White, (3, 1)), new (GameSide.White, (5, 1)), new (GameSide.White, (7, 1))
    });

    public bool TakeIfHas((int, int) position)
    {
        var pawn = PawnAt(position);
        if (pawn is not null)
        {
            Pawns.Remove(pawn);
            Taken.Add(pawn);
            return true;
        }

        return false;
    }

    public IEnumerable<Move> GetMovesForPawn(Pawn pawn)
    {
        var posX = pawn.Position.Item1;
        var posY = pawn.Position.Item2;
        var movesForPawn = Enumerable.Range(1, 2).SelectMany(n => new List<Move>
        {
            new(pawn.Position, (posX - n, posY + n)),
            new(pawn.Position, (posX + n, posY + n)),
            new(pawn.Position, (posX - n, posY - n)),
            new(pawn.Position, (posX + n, posY - n))
        }.Where(m => IsMoveCorrect(pawn.Side, m)));
        if (pawn.IsQueen)
        {
            movesForPawn = movesForPawn.Union(
                Enumerable.Range(3, 5).SelectMany(n => new List<Move>
                    {
                        new(pawn.Position, (posX - n, posY + n)),
                        new(pawn.Position, (posX + n, posY + n)),
                        new(pawn.Position, (posX - n, posY - n)),
                        new(pawn.Position, (posX + n, posY - n))
                    }.Where(m => IsMoveCorrect(pawn.Side, m))
                )
            );
        }

        return movesForPawn;
    }

    private bool IsMoveCorrect(GameSide side, Move move)
    {
        if (move.To.Item1 is < 0 or > 7 
            || move.To.Item2 is < 0 or > 7)
            return false;
        var diffX = move.To.Item1 - move.From.Item1;
        if (PawnAt(move.To) is not null)
        {
            return false;
        }

        var pawn = PawnAt(move.From)!;
        if(!pawn.IsQueen 
           && Math.Abs(diffX) == 1 
           && ((move.From.Item2 < move.To.Item2) && (pawn.Side == GameSide.Black) || 
               (move.From.Item2 > move.To.Item2) && (pawn.Side == GameSide.White))) 
            return false;

        var pawnsBetween = PositionsBetween(move).Select(PawnAt).ToList();
        //Console.WriteLine($"{move.From} {move.To} {pawnsBetween.Count}");
        if (pawnsBetween.Any(p => p != null && p.Side == side))
            return false;
        
        if(!pawn.IsQueen && pawnsBetween.Any())
            if (!pawnsBetween.Any(p => (p != null && p.Side != side)))
                return false;
        if (pawnsBetween.Any())
            move.Taking = true;
        return true;
    }

    public static List<(int, int)> PositionsBetween(Move move)
    {
        var positionsBetween = new List<(int, int)>();
        var diffX = move.To.Item1 - move.From.Item1;
        if (Math.Abs(diffX) > 1)
        {
            var diffY = move.To.Item2 - move.From.Item2;
            var unitDiffX = Math.Clamp(diffX, -1, 1);
            var unitDiffY = Math.Clamp(diffY, -1, 1);
            for (int i = 1; i < Math.Abs(diffX); i++)
            {
                positionsBetween.Add((move.From.Item1 + i * unitDiffX, move.From.Item2 + i * unitDiffY));
            }
        }

        return positionsBetween;
    }

    public Pawn? PawnAt((int, int) position)
    {
        return Pawns.FirstOrDefault(p => p.Position == position);
    }

    public void MakeMove(Move move)
    {
        var pawn = PawnAt(move.From)!;
        pawn.Position = move.To;
        if (((pawn.Side == GameSide.Black && pawn.Position.Item2 == 0)
            || (pawn.Side == GameSide.White && pawn.Position.Item2 == 7)) && 
            Math.Abs(move.From.Item1 - move.To.Item1) == 1)
            pawn.IsQueen = true;
    }
}