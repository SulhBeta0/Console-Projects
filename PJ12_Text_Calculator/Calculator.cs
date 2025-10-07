using PJ12_Text_Calculator;

internal static class Calculator
{
    /*
     * We can -
     * 1. Add & Substract integers (+/-) and decimals (+/-)
     * 
     * Future updates:
     * 2. Multiply
     * 3. Divide intergers, raise numbers to a power & square rooting integers (+/-) and decimals (+/-)
     * 
     * Big update:
     * 4. Use of parentheses :o
     */

    private static readonly List<double> MyNums = new();
    private static readonly List<string> MyOperations = new();
    private static double? _variablePostCalculations;

    internal enum Actions { MAS, MENOS, POR, ENTRE, ELEVADO, RESET, OFF, COMMANDS }

    internal static readonly string[] MyActions = Enum.GetNames<Actions>();
    internal const byte _maxConsecutivesOperations = 5;

    internal static void Update()
    {
        while (true)
        {
            string myText = TextProcessor.Text();

            if (myText == Actions.OFF.ToString()) return;
            else if (myText == Actions.RESET.ToString())
            {
                RestartValues(MyNums, MyOperations, true);
                Console.Clear();
                continue;
            }
            else if (myText == Actions.COMMANDS.ToString())
            {
                TextProcessor.Commands();
                continue;
            }

            var (numbers, operations) = TextProcessor.SplitText(myText, _variablePostCalculations, MyNums, MyOperations);
            if (numbers.Count == 0)
            {
                RestartValues(MyNums, MyOperations, false);
                continue;
            }

            Interactions(numbers, operations);
            RestartValues(MyNums, MyOperations, false);

            Result();
        }
    }

    static void Interactions(List<double> nums, List<string> ops)
    {
        if (nums.Count == 1 && !_variablePostCalculations.HasValue && ops.Count == 0)
        {
            _variablePostCalculations = nums[0];
            return;
        }

        double firstNumber = _variablePostCalculations.HasValue ? (double)_variablePostCalculations : nums[0];
        double secondNumber = _variablePostCalculations.HasValue ? nums[0] : nums[1];
        bool changingForm = false;
        short count = 0;

        if (_variablePostCalculations.HasValue) changingForm = true;
        else _variablePostCalculations = 0;

        while (count < ops.Count)
        {
            Actions myOperation = Enum.Parse<Actions>(ops[count]);
            switch (myOperation)
            {
                case Actions.MAS:
                    _variablePostCalculations = CalculatorOperations.Sum(firstNumber, secondNumber);
                    break;

                case Actions.MENOS:
                    _variablePostCalculations = CalculatorOperations.Substract(firstNumber, secondNumber);
                    break;

                case Actions.POR:
                    _variablePostCalculations = CalculatorOperations.Multiply(firstNumber, secondNumber);
                    break;

                case Actions.ENTRE:
                    _variablePostCalculations = CalculatorOperations.Divide(firstNumber, secondNumber);
                    break;

                case Actions.ELEVADO:
                    _variablePostCalculations = CalculatorOperations.RaiseToThePower(firstNumber, secondNumber);
                    break;
            }

            if (ops.Count < 2) break;
            if (count + 1 >= ops.Count) break;

            firstNumber = (double)_variablePostCalculations!;

            if (changingForm)
            {
                secondNumber = nums[count + 1];
                count++;
            }
            else
            {
                count++;
                secondNumber = nums[count + 1];
            }
        }
        Console.ResetColor();
    }

    static void Result()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("OUTPUT ==> " + _variablePostCalculations + "\n");
        Console.ResetColor();
    }

    static void RestartValues(List<double> numbers, List<string> operations, bool restartVariable)
    {
        if (restartVariable)
        {
            _variablePostCalculations = null;
            numbers.Clear();
            operations.Clear();
        }
        else
        {
            numbers.Clear();
            operations.Clear();
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
}
