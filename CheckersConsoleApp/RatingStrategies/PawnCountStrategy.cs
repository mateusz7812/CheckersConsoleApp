using System.Linq;

namespace CheckersConsoleApp.RatingStrategies
{ 

    public class PawnCountStrategy: IRatingStrategy
    {
        public int Rate(Board board)
        {
            return board.Pawns.Count(p => p.Side == GameSide.White) / board.Pawns.Count(p => p.Side == GameSide.Black);
        }
    }
}