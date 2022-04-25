
using CheckersConsoleApp;

Game game = new Game(
        new List<IPlayer>()
        {
                new ConsolePlayer{Side = GameSide.Black}, 
                new ConsolePlayer{Side = GameSide.White}
        });

game.Run();