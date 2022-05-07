using CheckersConsoleApp.Moves;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckersConsoleApp
{

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
                    if (!(win is null && draw == false))
                    {
                        Console.WriteLine("first");
                        break;
                    }
                    List<Move> availableMoves = Board.GetAvailableMoves(player.Side);
                    if (!availableMoves.Any())
                    {
                        Console.WriteLine("Second");
                        win = (int) GameSide.Black + (int) GameSide.White - player.Side;
                        break;
                    }

                    if (Board.Pawns.Any(p => p.IsQueen && p.Side == GameSide.Black)
                        && Board.Pawns.Any(p => p.IsQueen && p.Side == GameSide.White))
                        queenMovesCounter++;
                    if (queenMovesCounter > 15)
                    {
                        Console.WriteLine("third");
                        win = null;
                        draw = true;
                        break;
                    }

                    player.ShowBoard(Board);
                    move = player.GetMove(Board, availableMoves);
                    //Console.WriteLine($"{move.From} {move.To} {move.Taking}");
                    Board.MakeMove(move, true);
                    player.ShowBoard(Board);
                }
            }

            foreach (var player in Players)
            {
                player.AnnounceWinner(win, Board);
            }
        }
    }
}