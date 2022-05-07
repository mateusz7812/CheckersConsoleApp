using CheckersConsoleApp;
using CheckersConsoleApp.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CheckersWpfApp
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page, IGuiBoard
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private List<Ellipse> showedPawns = new List<Ellipse>();
        private List<Ellipse> showedAvailableMoves = new List<Ellipse>();
        private Thread gameThread;
        Game Game { get; set; }
        public Move? NextMove { get; private set; }

        public GamePage(Func<IGuiBoard, Game> createGame)
        {
            InitializeComponent();
            ArrangeBoardFields();
            Game = createGame(this);
            AllocConsole();
            gameThread = new Thread(() =>
            {
                try
                {
                    Game.Run();
                }
                catch (ThreadInterruptedException exception)
                {
                    Console.WriteLine("Game exit");
                }
            });
            gameThread.Start();
        }

        private void ArrangeBoardFields()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var label = new Label()
                    {
                        Background = i % 2 == j % 2 ? Brushes.AntiqueWhite : Brushes.DarkGray
                    };
                    grid.Children.Add(label);
                    Grid.SetColumn(label, i);
                    Grid.SetRow(label, j);
                }
            }
        }

        public void Display(Board board, List<Move> availableMoves = null)
        {
            Application.Current.Dispatcher.Invoke((Action) delegate {                 
                showedPawns.ForEach(pawn => grid.Children.Remove(pawn));
                showedPawns.Clear();
                showedAvailableMoves.ForEach(ellipse => grid.Children.Remove(ellipse));
                showedAvailableMoves.Clear();
                foreach (var pawn in board.Pawns)
                {
                    var elipse = new Ellipse() { Fill = pawn.Side == GameSide.Black ? Brushes.Black : Brushes.White, Margin = new Thickness(3)};
                    grid.Children.Add(elipse);
                    Grid.SetColumn(elipse, pawn.Position.Item1);
                    Grid.SetRow(elipse, 7 - pawn.Position.Item2);
                    showedPawns.Add(elipse);
                    if (pawn.IsQueen)
                    {
                        var crown = new Ellipse() { Stroke = pawn.Side == GameSide.White ? Brushes.Black : Brushes.White, Margin = new Thickness(8), StrokeThickness = 3 };
                        grid.Children.Add(crown);
                        Grid.SetColumn(crown, pawn.Position.Item1);
                        Grid.SetRow(crown, 7 - pawn.Position.Item2);
                        showedPawns.Add(crown);
                    }
                    if(availableMoves is not null) 
                        elipse.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs args) => ShowAvailableMoves(pawn, availableMoves));
                }
            });
        }

        private void ShowAvailableMoves(Pawn pawn, List<Move> availableMoves)
        {
            showedAvailableMoves.ForEach(ellipse => grid.Children.Remove(ellipse));
            showedAvailableMoves.Clear();
            foreach(var move in availableMoves.Where(move => move.From == pawn.Position)){
                var elipse = new Ellipse() { Fill = pawn.Side == GameSide.Black ? Brushes.Gray : Brushes.WhiteSmoke, Margin = new Thickness(3) };
                grid.Children.Add(elipse);
                Grid.SetColumn(elipse, move.To.Item1);
                Grid.SetRow(elipse, 7 - move.To.Item2);
                elipse.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs args) => { NextMove = move; });
                showedAvailableMoves.Add(elipse);
            }
        }

        public Move GetMove()
        {
            Console.WriteLine("waiting for move...");
            while (NextMove is null)
            {
                Thread.Sleep(500);
            }
            Move move = NextMove;
            NextMove = null;
            return move;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gameThread.Interrupt();
            //FreeConsole();
            this.NavigationService.GoBack();
        }

        public void AnnounceWinner(GameSide? gameSide, Board board)
        {
            Display(board);
            Application.Current.Dispatcher.Invoke((Action) delegate
            {
                label.Content = $"{((gameSide is null) ? "no one" : gameSide )} wins";
            });
        }
    }
}
