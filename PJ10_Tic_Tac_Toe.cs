//Finished on May 28th, 2025 -

public class Program
{
    public static void Main()
    {
        Game juego = new();

        Console.WriteLine("Your simbol is => " + juego.MySymbol);
        Thread.Sleep(1200);
        Console.Clear();

        Console.WriteLine("Tic-Tac-Toe");
        Tile.UpdateGrid();
        juego.Start();
    }
}

public class Game
{
    private static readonly Random _rng = new();
    private bool _myTurn;

    public Tile.Symbols MySymbol { get; private set; }
    public Tile.Symbols IaSymbol { get; private set; }

    public Game()
    {
        if(_rng.NextSingle() < 0.5)
        {
            MySymbol = Tile.Symbols.O;
            IaSymbol = Tile.Symbols.X;
            _myTurn = true;
        }
        else {
            MySymbol = Tile.Symbols.X;
            IaSymbol = Tile.Symbols.O;
            _myTurn = false;
        }
    }

    public void Start()
    {
        int myDecision;
        int iaDecision;

        while (!Tile.GameState(Convert.ToChar(MySymbol)) && !Tile.GameState(Convert.ToChar(IaSymbol)))
        {
            Thread.Sleep(1500);
            if (_myTurn)
            {
                Console.Write("Your turn! Enter number (0-8) => ");
                myDecision = int.Parse(Console.ReadLine() ?? "0");

                while (!Tile.PlaceAvailable(MySymbol, IaSymbol, myDecision))
                {  
                    Console.Write("Place taken! => ");
                    myDecision = int.Parse(Console.ReadLine() ?? "0");
                }
                Thread.Sleep(1000);

                Console.Clear();
                Console.WriteLine("Tic-Tac-Toe");
                Tile.UpdateGrid(myDecision, Tile.PlacingTile(MySymbol));

                _myTurn = false;
            }
            else {
                Console.WriteLine("Ia turn!");
                iaDecision = _rng.Next(0, 10);

                while (!Tile.PlaceAvailable(IaSymbol, MySymbol, iaDecision))
                {
                    iaDecision = _rng.Next(0, 10);
                }
                Thread.Sleep(1000);

                Console.Clear();
                Console.WriteLine("Tic-Tac-Toe");
                Tile.UpdateGrid(iaDecision, Tile.PlacingTile(IaSymbol));

                _myTurn = true;
            }
        }
        
    }
}

public class Tile
{
    private static char[,] Grid = new char[3, 3]
    {
        { '*', '*', '*' },
        { '*', '*', '*' },
        { '*', '*', '*' }
    };

    public enum Symbols { X = 'X', O = 'O', EMPTY = '*'}

    public char Tipo { get; private set; }
    public ConsoleColor Color { get; private set; }

    public Tile(ConsoleColor background, char type)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Color = background;
        Tipo = type;
    }

    public static Tile PlacingTile(Symbols symbol)
    {
        switch(symbol)
        {
            case Symbols.X:
                return new Tile(ConsoleColor.DarkRed, 'X');

            case Symbols.O:
                return new Tile(ConsoleColor.DarkBlue, 'O');

            default:
                return new Tile(ConsoleColor.Black, '*');
        }
    }

    public static bool GameState(char symbol)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Grid[i,0] == symbol && Grid[i, 1] == symbol && Grid[i, 2] == symbol)
            {
                Console.WriteLine("The -{0}- has win!", symbol);
                return true;
            }
        }
        
        for (int j = 0; j < 3; j++)
        {
            if (Grid[0,j] == symbol && Grid[1, j] == symbol && Grid[2, j] == symbol)
            {
                Console.WriteLine("The -{0}- has win!", symbol);
                return true;
            }
        }

        if (Grid[0,0] == symbol && Grid[1, 1] == symbol && Grid[2, 2] == symbol)
        {
            Console.WriteLine("The -{0}- has win!", symbol);
            return true;
        }
        
        if (Grid[2,0] == symbol && Grid[1, 1] == symbol && Grid[0, 2] == symbol)
        {
            Console.WriteLine("The -{0}- has win!", symbol);
            return true;
        }

        return false;
    }

    public static bool PlaceAvailable(Symbols mySymbol, Symbols rivalSymbol, int location)
    {
        if (location < 0 || location > 8) return false;

        int row = location / 3;
        int column = location % 3;

        return Grid[row, column] != Convert.ToChar(mySymbol) &&
               Grid[row, column] != Convert.ToChar(rivalSymbol);
    }

    public static void UpdateGrid()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Console.Write("{0:15} ", PlacingTile(Symbols.EMPTY).Tipo);
            }
            Console.WriteLine();
        }
    }
    
    public static void UpdateGrid(int location, Tile tipo)
    {
        int row = location / 3;
        int column = location % 3;

        Grid[row, column] = tipo.Tipo;
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (i == row && j == column)
                {
                    Console.BackgroundColor = tipo.Color;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else { Console.ResetColor(); }

                Console.Write("{0:15} ", Grid[i, j]);
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }
}