using CheckersConsoleApp.Moves;
using System.Collections.Generic;

namespace CheckersConsoleApp
{

    public interface IPlayer
    {
        public GameSide Side { get; }
        public Move GetMove(Board board, List<Move> availableMoves);
        void AnnounceWinner(GameSide? gameSide, Board board);
        void ShowBoard(Board board);
    }
}