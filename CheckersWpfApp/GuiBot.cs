using CheckersConsoleApp;
using CheckersConsoleApp.Moves;
using CheckersConsoleApp.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWpfApp
{
    class GuiBot : IPlayer
    {
        private Bot bot;

        public GuiBot(IGuiBoard guiBoard, Bot bot)
        {
            GuiBoard = guiBoard;
            this.bot = bot;
        }

        public GameSide Side => bot.Side;

        public IGuiBoard GuiBoard { get; }

        public void AnnounceWinner(GameSide? gameSide, Board board)
        {
            GuiBoard.AnnounceWinner(gameSide, board);
        }

        public Move GetMove(Board board, List<Move> availableMoves)
        {
            return bot.GetMove(board, availableMoves);
        }

        public void ShowBoard(Board board)
        {
            GuiBoard.Display(board);
        }
    }
}
