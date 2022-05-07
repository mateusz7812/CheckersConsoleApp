using CheckersConsoleApp;
using CheckersConsoleApp.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CheckersWpfApp
{
    class GuiPlayer : IPlayer
    {
        public IGuiBoard GuiBoard { get; }
        public GameSide Side { get; init; }

        public GuiPlayer(IGuiBoard guiBoard, GameSide side)
        {
            GuiBoard = guiBoard;
            Side = side;
        }

        public void AnnounceWinner(GameSide? gameSide, Board board)
        {
            GuiBoard.AnnounceWinner(gameSide, board);
        }

        public Move GetMove(Board board, List<Move> availableMoves)
        {
            //Dispatcher.Invoke(new Action(() => ), DispatcherPriority.ApplicationIdle);
            GuiBoard.Display(board, availableMoves);
            return GuiBoard.GetMove();
        }

        public void ShowBoard(Board board)
        {
            GuiBoard.Display(board);
        }
    }
}
