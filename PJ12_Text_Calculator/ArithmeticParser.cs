namespace PJ12_Text_Calculator;
using static Calculator;

#pragma warning disable CA1416
public class ArithmeticParser
{
    private static readonly Dictionary<string, short> _precedence = new()
    {
        {"(", 0 }, //temporal
        {Actions.PLUS.ToString(), 1 },
        {Actions.MINUS.ToString(), 1 },
        {Actions.TIMES.ToString(), 2 },
        {Actions.DIVIDED.ToString(), 3 },
        {Actions.POWER.ToString(), 4 } //temporal

    };
    private static readonly List<string> _op_stack = [];
    private static readonly List<string> _solving_stack = [];
    private readonly List<string> _output = [];

    public double? VariablePostCalculations { get; private set; }


    public void RestartValues() => VariablePostCalculations = null;

    public void ParsingOperations(string input)
    {
        string[] tokens = input.Split(" ", StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
        if (!TokensAuthentication(tokens))
        {
            TextProcessor.Messages("Syntax Error!\n\n", ConsoleColor.Red);
            Console.Beep(450, 300);
            RestartValues();
            return;
        }

        if (tokens.Length == 1 && (!VariablePostCalculations.HasValue || VariablePostCalculations.HasValue))
        {
            VariablePostCalculations = double.Parse(tokens[0]);
            return;
        }

        short operationInStack = 0, inputOperation = 0;
        int counter;

        if (!VariablePostCalculations.HasValue)
        {
            _output.Add(tokens[0]);
            _op_stack.Add(tokens[1]);
        }
        else
        {
            _output.Add(VariablePostCalculations.ToString()!);
            _op_stack.Add(tokens[0]);
        }

        int startingIndex = !VariablePostCalculations.HasValue ? 2 : 1;
        for (int i = startingIndex; i < tokens.Length; i++)
        {
            if (double.TryParse(tokens[i], out _))
            {
                _output.Add(tokens[i]);
                continue;
            }

            _precedence.TryGetValue(tokens[i].ToUpper(), out inputOperation);
            counter = _op_stack.Count - 1;
            _precedence.TryGetValue(_op_stack[counter].ToUpper(), out operationInStack);

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

        //foreach (var item in _output)
        //{
        //    Console.Write(item + " ");
        //}
        //Console.WriteLine("\n");

        SolvingOutput(_output);

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
        double firstNumber = 0;
        double secondNumber = 0;
        int counter = 0, countForOperations = 0;
        Actions myOperation;

        if (!VariablePostCalculations.HasValue) VariablePostCalculations = 0;
        _solving_stack.Clear();

        while (counter < list.Count)
        {
            _solving_stack.Add(list[counter]);
            if (!MyActions.Contains(_solving_stack[countForOperations].ToUpper()))
            {
                countForOperations++;
                counter++;
                continue;
            }

            firstNumber = double.Parse(_solving_stack[countForOperations - 2]);
            secondNumber = double.Parse(_solving_stack[countForOperations - 1]);

            myOperation = Enum.Parse<Actions>(_solving_stack[countForOperations].ToUpper());
            switch (myOperation)
            {
                case Actions.PLUS:
                    VariablePostCalculations = CalculatorOperations.Sum(firstNumber, secondNumber);
                    break;

                case Actions.MINUS:
                    VariablePostCalculations = CalculatorOperations.Substract(firstNumber, secondNumber);
                    break;

                case Actions.TIMES:
                    VariablePostCalculations = CalculatorOperations.Multiply(firstNumber, secondNumber);
                    break;

                case Actions.DIVIDED:
                    VariablePostCalculations = CalculatorOperations.Divide(firstNumber, secondNumber);
                    break;
            }

            _solving_stack.RemoveRange(countForOperations-2, 3);
            _solving_stack.Add(VariablePostCalculations.ToString()!);

            countForOperations = _solving_stack.IndexOf(_solving_stack[^1]) + 1;
            counter++;
        }
        Console.ResetColor();
    }

    private bool TokensAuthentication(string[] arr)
    {
        short countingOperations = 0;
        bool isAuthenticOperator = false;
        bool isNumber = false;
        for (int i = 0; i < arr.Length; i++)
        {
            if (VariablePostCalculations.HasValue && arr.Length > 1)
            {
                isAuthenticOperator = MyActions.Contains(arr[i].ToUpper());
                isNumber = i % 2 != 0 && double.TryParse(arr[i], out _);

                if (double.TryParse(arr[0], out _)) RestartValues();
            }
            else if (
                VariablePostCalculations.HasValue &&
                arr.Length == 1 &&
                double.TryParse(arr[i], out _)) return true;

            if (!VariablePostCalculations.HasValue)
            {
                isAuthenticOperator = i % 2 != 0 && MyActions.Contains(arr[i].ToUpper());
                isNumber = double.TryParse(arr[i], out _);
            }

            if (!isNumber && !isAuthenticOperator) return false;

            if (isAuthenticOperator) countingOperations++;
            if (isAuthenticOperator && countingOperations < 1 || countingOperations > _maxConsecutivesOperations) return false;
        }
        return true;
    }

}
