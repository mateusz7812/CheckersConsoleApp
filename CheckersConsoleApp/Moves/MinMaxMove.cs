namespace CheckersConsoleApp.Moves;

public class MinMaxMove: Move
{
    public MinMaxMove(Move move, int rating) : base(move.From, move.To, move.Taking)
    {
        Rating = rating;
    }
    
    public int Rating { get; set; }
}