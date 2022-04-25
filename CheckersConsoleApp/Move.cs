namespace CheckersConsoleApp;

public class Move
{
    public Move((int, int) @from, (int, int) to)
    {
        From = @from;
        To = to;
    }

    public (int, int) From { get; }
    public (int, int) To { get; }
    public bool Taking { get; set; } = false;
}