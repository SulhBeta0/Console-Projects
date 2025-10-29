namespace PJ12_Text_Calculator;
using static Calculator;

#pragma warning disable CA1416
public class ArithmeticParser
{
    private static readonly Dictionary<string, short> _precedence = new()
    {
        {Actions.PLUS.ToString(), 1 },
        {Actions.MINUS.ToString(), 1 },
        {Actions.TIMES.ToString(), 2 },
        {Actions.DIVIDED.ToString(), 2 }
    };
    private static readonly List<string> _op_stack = [];
    private readonly List<string> _output = [];

    public double? VariablePostCalculations { get; private set; }


    public void RestartValues() => VariablePostCalculations = null;

    public void ParsingOperations(string input)
    {
        string[] tokens = input.Split(" ", StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
        
        if (!TokensAuthentication(tokens) && VariablePostCalculations.HasValue) return;
        else if (!TokensAuthentication(tokens))
        {
            TextProcessor.Messages("Syntax Error!\n\n", ConsoleColor.Red);
            Console.Beep(450, 300);
            return;
        }

        if (tokens.Length == 1 && !VariablePostCalculations.HasValue || VariablePostCalculations.HasValue)
        {
            VariablePostCalculations = double.Parse(tokens[0]);
            return;
        }

        short operationInStack = 0, inputOperation = 0;
        int counter;

        _output.Add(tokens[0]);
        _op_stack.Add(tokens[1]);

        //2 plus 3 times 2 minus 2 times 6 plus 8
        //2 + 3 * 2 - 2 * 6 + 8
        for (int i = 2; i < tokens.Length; i++)
        {
            if (double.TryParse(tokens[i], out _))
            {
                _output.Add(tokens[i]);
                continue;
            }

            _precedence.TryGetValue(tokens[i], out inputOperation);
            counter = _op_stack.Count - 1;
            _precedence.TryGetValue(_op_stack[counter], out operationInStack);

            if (inputOperation > operationInStack)
            {
                _op_stack.Add(tokens[i]);
                continue;
            }

            while (inputOperation <= operationInStack && _op_stack.Count > 0)
            {
                _output.Add(_op_stack[counter]);
                _op_stack.RemoveAt(counter);
                counter--;
            }
            if (_op_stack.Count == 0) _op_stack.Add(tokens[i]);
        }

        while (_op_stack.Count > 0)
        {
            _output.Add(_op_stack.Last<String>());
            _op_stack.RemoveAt(_op_stack.Count - 1);
        }
        SolvingOutput(_output);

        //foreach (var item in _output)
        //{
        //    Console.Write(item + " ");
        //}
        //Console.WriteLine("\n");

        _output.Clear();
    }

    public void Result()
    {
        if (VariablePostCalculations == null) return;

        TextProcessor.Messages("OUTPUT ==> " + VariablePostCalculations + "\n\n", ConsoleColor.Red);
        Console.ResetColor();
    }

    private void SolvingOutput(List<string> list)
    {
        //2 3 2 * + 2 6 * - 8 +
        //2 3 2 times plus 2 6 times minus 8 plus

        //double firstNumber = 0;
        //double secondNumber = 0;
        //bool changingForm = false;
        short count = 0;

        //if (VariablePostCalculations.HasValue) changingForm = true;
        //else VariablePostCalculations = 0;

        while (count < list.Count)
        {
            if (MyActions.Contains(list[count].ToUpper()))
            {
                
            }
            //Actions myOperation = Enum.Parse<Actions>(ops[count]);
            //switch (myOperation)
            //{
            //    case Actions.PLUS:
            //        VariablePostCalculations = CalculatorOperations.Sum(firstNumber, secondNumber);
            //        break;

            //    case Actions.MINUS:
            //        VariablePostCalculations = CalculatorOperations.Substract(firstNumber, secondNumber);
            //        break;

            //    case Actions.TIMES:
            //        VariablePostCalculations = CalculatorOperations.Multiply(firstNumber, secondNumber);
            //        break;

            //    case Actions.DIVIDED:
            //        VariablePostCalculations = CalculatorOperations.Divide(firstNumber, secondNumber);
            //        break;
            //}

            //if (ops.Count < 2) break;
            //if (count + 1 >= ops.Count) break;

            //firstNumber = (double)VariablePostCalculations!;

            //if (changingForm)
            //{
            //    secondNumber = nums[count + 1];
            //    count++;
            //}
            //else
            //{
            //    count++;
            //    secondNumber = nums[count + 1];
        }
        Console.ResetColor();
    }

    private static bool TokensAuthentication(string[] arr)
    {
        short countingOperations = 0;
        bool isAuthenticOperator;
        for (int i = 0; i < arr.Length; i++)
        {
            isAuthenticOperator = MyActions.Contains(arr[i % 2].ToUpper());

            if (!double.TryParse(arr[i], out _) && !isAuthenticOperator) return false;

            if (isAuthenticOperator) countingOperations++;
            if (isAuthenticOperator && countingOperations < 1 || countingOperations > _maxConsecutivesOperations) return false;
        }
        return true;
    }

}
