using PJ12_Text_Calculator;

internal class Calculator
{
    public static string[] MyActions => Enum.GetNames<Actions>();
    private readonly ArithmeticParser parser = new();

    internal enum Actions { PLUS, MINUS, TIMES, DIVIDED, POWER, SQRT, RESET, OFF, HELP }

    internal const byte _maxConsecutivesOperations = 6;

    internal void Update()
    {
        while (true)
        {
            string myText = TextProcessor.Text();

            if (myText == Actions.OFF.ToString()) return;
            else if (myText == Actions.RESET.ToString())
            {
                parser.RestartValues();
                Console.Clear();
                continue;
            }
            else if (myText == Actions.HELP.ToString())
            {
                TextProcessor.Commands(MyActions);
                continue;
            }

            parser.ParsingOperations(myText);
            parser.Result();
        }
    }

}

internal static class CalculatorOperations
{
    internal static double Sum(double x, double y) => x + y;
    internal static double Substract(double x, double y) => x - y;
    internal static double Multiply(double x, double y) => x * y;
    internal static double Divide(double x, double y)
    {
        if (y == 0) return double.NaN;
        return x / y;
    }
    internal static double RaiseToThePower(double x, double exponent) => Math.Pow(x, exponent);

    //internal static double SquareRooting(double x)
    //{
    //    if (x < 0) return double.NaN;
    //    return Math.Sqrt(x);
    //}
}
