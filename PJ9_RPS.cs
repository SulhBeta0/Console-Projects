//Finished on May 16th, 2025 -

public class Program
{
    public static void Main()
    {
        Game juego = new();
        while (true)
        {
            Console.Write("Stone, paper or scissors? => ");
            string decision = Console.ReadLine()?.ToUpper() ?? "error";

            if (String.IsNullOrEmpty(decision)) break;
            else juego.Start(decision);

            Thread.Sleep(2000);
            Console.ResetColor();
            Console.Clear();
        }
    }
}

public class Game
{
    private enum Moves { STONE, PAPER, SCISSORS }
    private static readonly Random _rng = new();

    public string UserDecision { get; private set; } = String.Empty;
    public string IaDecision { get; private set; } = String.Empty;

    public void Start(string move)
    {
        UserDecision = move;
        IaDecision = Enum.GetNames(typeof(Moves))[_rng.Next(3)];

        Console.ForegroundColor = ConsoleColor.Cyan;
        if (UserDecision == IaDecision) {
            Console.WriteLine("Mine: {0}\nIa: {1}\nIt's a tie!", UserDecision, IaDecision);
            return;
        }

        switch(Enum.Parse(typeof(Moves), move))
        {
            case Moves.STONE:
                if(IaDecision == Moves.SCISSORS.ToString())
                {
                    Console.WriteLine("Mine: {0}\nIa: {1}\nUser Wins!", UserDecision, IaDecision);
                }
                else { Console.WriteLine("Mine: {0}\nIa: {1}\nIa Wins!", UserDecision, IaDecision); }
                break;
            
            case Moves.PAPER:
                if (IaDecision == Moves.STONE.ToString())
                {
                    Console.WriteLine("Mine: {0}\nIa: {1}\nUser Wins!", UserDecision, IaDecision);
                }
                else { Console.WriteLine("Mine: {0}\nIa: {1}\nIa Wins!", UserDecision, IaDecision); }
                break;

            case Moves.SCISSORS:
                if (IaDecision == Moves.PAPER.ToString())
                {
                    Console.WriteLine("Mine: {0}\nIa: {1}\nUser Wins!", UserDecision, IaDecision);
                }
                else { Console.WriteLine("Mine: {0}\nIa: {1}\nIa Wins!", UserDecision, IaDecision); }
                break;
        }
    }
}
