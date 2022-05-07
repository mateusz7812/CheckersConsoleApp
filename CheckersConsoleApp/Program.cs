
using CheckersConsoleApp.Players;
using CheckersConsoleApp.RatingStrategies;
using System.Collections.Generic;
namespace CheckersConsoleApp
{
    class Program
    {
        public static void Main()
        {
            Game game = new Game(
                    new List<IPlayer>()
                    {
                    //new ConsolePlayer{Side = GameSide.Black}, 
                    new MinMaxBot(new CirclesStrategy(), GameSide.Black, 4),
                    //new ConsolePlayer{Side = GameSide.White}, 
                    new MinMaxBot(new CirclesStrategy(), GameSide.White, 4)
                    });

            game.Run();
        }
    }
}