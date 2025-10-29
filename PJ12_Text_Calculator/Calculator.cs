using PJ12_Text_Calculator;

internal class Calculator
{
    /*
     * We can/have -
     * 1. Add & Substract (+/-) integers and decimals.
     * 2. Multiply.
     * 3. Divide and raise to the power (+/-) integers and decimals.
     * 
     * Future updates:
     * 4. Arithmetic hierarchy.
     * 5. Use parentheses.
     * 6. Square rooting (i'll try).
     */

    //private static readonly List<double> MyNums = new();
    //private static readonly List<string> MyOperations = new();
    public static string[] MyActions => Enum.GetNames<Actions>();
    private readonly ArithmeticParser parser = new();
    //private static readonly string[] MyActions = Enum.GetNames<Actions>();
    //private static double? _variablePostCalculations;

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

            //Console.ReadKey(true);
            //var (numbers, operations) = TextProcessor.SplitText(myText, _variablePostCalculations, MyNums, MyOperations);
            //if (numbers.Count == 0)
            //{
            //    RestartValues(false);
            //    continue;
            //}

            //Interactions(numbers, operations);
            //RestartValues(false);
        }
    }

}
//internal static class Calculator
//{
//    /*
//     * We can -
//     * 1. Add & Substract integers (+/-) and decimals (+/-)
//     * 2. Multiply
//     * 3. Divide, raise numbers to a power & square rooting integers (+/-) and decimals (+/-)
//     * 
//     * Future updates:
//     * 4. Arithmetic hierarchy.
//     * 5. Use of parentheses :o
//     */

//    private static readonly List<double> MyNums = new();
//    private static readonly List<string> MyOperations = new();
//    private static double? _variablePostCalculations;

//    internal enum Actions { PLUS, MINUS, TIMES, DIVIDED, POWER, SQRT, RESET, OFF, HELP }

//    internal static readonly string[] MyActions = Enum.GetNames<Actions>();
//    internal const byte _maxConsecutivesOperations = 6;

//    internal static void Update()
//    {
//        while (true)
//        {
//            string myText = TextProcessor.Text();

//            if (myText == Actions.OFF.ToString()) return;
//            else if (myText == Actions.RESET.ToString())
//            {
//                RestartValues(MyNums, MyOperations, true);
//                Console.Clear();
//                continue;
//            }
//            else if (myText == Actions.HELP.ToString())
//            {
//                TextProcessor.Commands();
//                continue;
//            }

//            var (numbers, operations) = TextProcessor.SplitText(myText, _variablePostCalculations, MyNums, MyOperations);
//            if (numbers.Count == 0)
//            {
//                RestartValues(MyNums, MyOperations, false);
//                continue;
//            }

//            Interactions(numbers, operations);
//            RestartValues(MyNums, MyOperations, false);

//            Result();
//        }
//    }

//    static void Interactions(List<double> nums, List<string> ops)
//    {
//        if (nums.Count == 1 && !_variablePostCalculations.HasValue && ops.Count == 0)
//        {
//            _variablePostCalculations = nums[0];
//            return;
//        }

//        double firstNumber = _variablePostCalculations.HasValue ? (double)_variablePostCalculations : nums[0];
//        double secondNumber = _variablePostCalculations.HasValue ? nums[0] : nums[1];
//        bool changingForm = false;
//        short count = 0;

//        if (_variablePostCalculations.HasValue) changingForm = true;
//        else _variablePostCalculations = 0;

//        while (count < ops.Count)
//        {
//            Actions myOperation = Enum.Parse<Actions>(ops[count]);
//            switch (myOperation)
//            {
//                case Actions.PLUS:
//                    _variablePostCalculations = CalculatorOperations.Sum(firstNumber, secondNumber);
//                    break;

//                case Actions.MINUS:
//                    _variablePostCalculations = CalculatorOperations.Substract(firstNumber, secondNumber);
//                    break;

//                case Actions.TIMES:
//                    _variablePostCalculations = CalculatorOperations.Multiply(firstNumber, secondNumber);
//                    break;

//                case Actions.DIVIDED:
//                    _variablePostCalculations = CalculatorOperations.Divide(firstNumber, secondNumber);
//                    break;

//                case Actions.POWER:
//                    _variablePostCalculations = CalculatorOperations.RaiseToThePower(firstNumber, secondNumber);
//                    break;
//                //case Actions.SQRT:
//                //    _variablePostCalculations = CalculatorOperations.RaiseToThePower(firstNumber, secondNumber);
//                //    break;
//            }

//            if (ops.Count < 2) break;
//            if (count + 1 >= ops.Count) break;

//            firstNumber = (double)_variablePostCalculations!;

//            if (changingForm)
//            {
//                secondNumber = nums[count + 1];
//                count++;
//            }
//            else {
//                count++;
//                secondNumber = nums[count + 1];
//            }
//        }
//        Console.ResetColor();
//    }

//    static void Result()
//    {
//        Console.ForegroundColor = ConsoleColor.Red;
//        Console.WriteLine("OUTPUT ==> " + _variablePostCalculations + "\n");
//        Console.ResetColor();
//    }

//    static void RestartValues(List<double> numbers, List<string> operations, bool restartVariable)
//    {
//        if (restartVariable)
//        {
//            _variablePostCalculations = null;
//            numbers.Clear();
//            operations.Clear();
//        }
//        else {
//            numbers.Clear();
//            operations.Clear();
//        }
//    }

//}

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
