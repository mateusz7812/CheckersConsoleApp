using CheckersConsoleApp;
using CheckersConsoleApp.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWpfApp
{
    public interface IGuiBoard
    {
        void Display(Board board, List<Move> availableMoves = null);
        Move GetMove();
        void AnnounceWinner(GameSide? gameSide, Board board);
    }
}
