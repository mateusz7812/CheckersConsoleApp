using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CheckersConsoleApp;
using CheckersConsoleApp.Players;
using CheckersConsoleApp.RatingStrategies;
//using CheckersConsoleApp;

namespace CheckersWpfApp
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        private readonly Frame mainFrame;

        public MenuPage(Frame _mainFrame)
        {
            InitializeComponent();
            mainFrame = _mainFrame;
        }

        private void AIvsAIButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GamePage(page => new Game(
                            new List<IPlayer>()
                            {
                                new MinMaxBot(new CirclesStrategy(), GameSide.Black, 4),
                                new GuiBot(page, new MinMaxBot(new CirclesStrategy(), GameSide.White, 4))
                            }
                    )
                )
            );
        }
        private void GuiVsAIButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GamePage(page => new Game(
                            new List<IPlayer>()
                            {
                                new MinMaxBot(new CirclesStrategy(), GameSide.Black, 4),
                                new GuiPlayer(page, GameSide.White)
                            }
                    )
                )
            );
        }

        private void GuiVsConsole_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GamePage(page => new Game(
                            new List<IPlayer>()
                            {
                                new ConsolePlayer{Side = GameSide.Black},
                                new GuiPlayer(page, GameSide.White)
                            }
                    )
                )
            );
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
