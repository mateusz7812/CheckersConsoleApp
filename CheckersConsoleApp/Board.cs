using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersConsoleApp.Moves;

namespace CheckersConsoleApp
{

    public class Board
{
    public Board(List<Pawn> pawns, List<Pawn>? taken = null)
    {
        Pawns = pawns;
        Taken = taken ?? new List<Pawn>();
    }

    public List<Pawn> Pawns { get; }
    public List<Pawn> Taken { get; }

    public static Board Initial = new(new List<Pawn>()
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
        if (!pawn.IsQueen
           && Math.Abs(diffX) == 1
           && ((move.From.Item2 < move.To.Item2) && (pawn.Side == GameSide.Black) ||
               (move.From.Item2 > move.To.Item2) && (pawn.Side == GameSide.White)))
            return false;

        var positionsBetween = PositionsBetween(move);
        var pawnsBetween = positionsBetween.Select(PawnAt).Where(n => n is not null).ToList();
        if (pawnsBetween.Count > 1)
            return false;
        if (pawnsBetween.Any())
        {
            if (PositionsBetween(new Move(pawnsBetween [0]!.Position, move.To)).Count != 0)
            {
                return false;
            }
        }
        if (pawnsBetween.Any(p => p != null && p.Side == side))
            return false;

        if (!pawn.IsQueen)
        {
            if (!pawnsBetween.Any() && positionsBetween.Any())
                return false;
        }
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


    public List<Move> GetAvailableMoves(GameSide side)
    {
        var availableMoves = this.Pawns
            .FindAll(p => p.Side == side)
            .SelectMany(this.GetMovesForPawn)
            .ToList();
        if (availableMoves.Count == 1)
            return availableMoves;
        if (!availableMoves.Any(m => m.Taking))
            return availableMoves;

        var availableTakes = availableMoves.Where(m => m.Taking).ToList();
        var availableTakesWithRates = availableTakes.Select(take =>
            (
                take,
                CountTakes(PawnAt(take.From)!.Side, take)
            )
        ).ToList();
        var orderByDescending = availableTakesWithRates.OrderByDescending(t => t.Item2).ToList();
        return orderByDescending
            .TakeWhile(o => o.Item2 == orderByDescending.First().Item2)
            .Select(i => i.Item1)
            .ToList();
    }

    public Pawn MakeMove(Move move, bool real = false)
    {
        MakeSingleMove(move);

        var pawn = PawnAt(move.To)!;
        var nextMoves = GetAvailableMoves(pawn.Side);
        if (nextMoves.Count() != 1)
            return pawn;
        var nextMove = nextMoves [0];
        if (move.Taking && nextMove.Taking)
        {
            if (real)
                Console.WriteLine($"combo: {move.To} {nextMove.To}");
            MakeMove(nextMove);
        }
        return pawn;
    }

    private void MakeSingleMove(Move move)
    {
        var pawn = PawnAt(move.From)!;
        pawn.Position = move.To;
        if (((pawn.Side == GameSide.Black && pawn.Position.Item2 == 0)
             || (pawn.Side == GameSide.White && pawn.Position.Item2 == 7)) &&
            Math.Abs(move.From.Item1 - move.To.Item1) == 1)
            pawn.IsQueen = true;
        if (move.Taking)
        {
            var positionsBetween = PositionsBetween(move);
            foreach (var position in positionsBetween)
            {
                TakeIfHas(position);
            }
        }
    }

    public Board Clone()
    {
        return new Board(Pawns.Select(p => p.Clone()).ToList(), Taken.Select(t => t.Clone()).ToList());
    }

    public int CountTakes(GameSide side, Move move)
    {
        var boardCopy = Clone();
        boardCopy.MakeSingleMove(move);
        var availableMoves = boardCopy.GetAvailableMoves(side);
        var takes = availableMoves.Where(m => m.Taking).ToList();
        var counts = takes.Select(t => boardCopy.CountTakes(side, t)).DefaultIfEmpty(0).ToList();
        return 1 + counts.Max();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(" ");
        Enumerable.Range(0, 8).ToList().ForEach(n => builder.AppendFormat($"  {n}"));
        builder.AppendLine();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (j == 0)
                {
                    builder.Append(7 - i);
                }

                var pawn = PawnAt((j, 7 - i));
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
                    fieldSymbol = sideName [0].ToString();
                }

                builder.AppendFormat($"  {fieldSymbol}");
            }

            builder.AppendLine();
        }
        return builder.ToString();
    } 
}
}
