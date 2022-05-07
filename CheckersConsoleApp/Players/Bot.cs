using CheckersConsoleApp.Moves;
using CheckersConsoleApp.RatingStrategies;
using System.Collections.Generic;

namespace CheckersConsoleApp
{

    public abstract class Bot : IPlayer
    {
        protected Bot(IRatingStrategy ratingStrategy, GameSide side)
        {
            RatingStrategy = ratingStrategy;
            Side = side;
        }

        public IRatingStrategy RatingStrategy { get; set; }
        public GameSide Side { get; init; }
        public abstract Move GetMove(Board board, List<Move> availableMoves);

        public abstract void AnnounceWinner(GameSide? gameSide, Board board);

        public void ShowBoard(Board board)
        {
            //throw new System.NotImplementedException();
        }
    }
}