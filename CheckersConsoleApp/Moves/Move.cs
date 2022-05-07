namespace CheckersConsoleApp.Moves
{
    public class Move
    {
        public Move((int, int) @from, (int, int) to, bool moveTaking = false)
        {
            From = @from;
            To = to;
            Taking = moveTaking;
        }

        public (int, int) From { get; }
        public (int, int) To { get; }
        public bool Taking { get; set; } = false;

        public override string ToString()
        {
            return $"{From}=>{To} {Taking}";
        }
    }
}