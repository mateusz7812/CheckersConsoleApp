
using CheckersConsoleApp;
using CheckersConsoleApp.Players;
using CheckersConsoleApp.RatingStrategies;

Game game = new Game(
        new List<IPlayer>()
        {
                //new ConsolePlayer{Side = GameSide.Black}, 
                new MinMaxBot(new CirclesStrategy(), GameSide.Black, 4),
                new ConsolePlayer{Side = GameSide.White}, 
                //new MinMaxBot(new CirclesStrategy(), GameSide.White, 4)
        });

game.Run();