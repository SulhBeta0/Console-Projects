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
        {Actions.DIVIDED.ToString(), 3 },
        {Actions.POWER.ToString(), 4 }

    };
    private static readonly List<string> _op_stack = [];
    private static readonly List<string> _solving_stack = [];
    private readonly List<string> _output = [];

    public double? VariablePostCalculations { get; private set; }


    public void RestartValues() => VariablePostCalculations = null;

    public void ParsingOperations(string[] tokens)
    {
        short operatorInStackValue = 0, inputOperatorValue = 0;
        int counter, startingIndex;
        bool hasReachedAParenthesis = false;
        bool hasExitAParenthesis = false;

        if (!VariablePostCalculations.HasValue)
        {
            _output.Add(tokens[0]);
            _op_stack.Add(tokens[1]);
            startingIndex = 2;
        }
        else {
            _output.Add(VariablePostCalculations.ToString()!);
            _op_stack.Add(tokens[0]);
            startingIndex = 1;
        }

        for (int i = startingIndex; i < tokens.Length; i++)
        {
            if (double.TryParse(tokens[i], out _))
            {
                _output.Add(tokens[i]);
                continue;
            }

            _precedence.TryGetValue(tokens[i].ToUpper(), out inputOperatorValue);
            counter = _op_stack.Count - 1;
            _precedence.TryGetValue(_op_stack[counter].ToUpper(), out operatorInStackValue);

            if (tokens[i] == "(")
            {
                hasReachedAParenthesis = true;
                hasExitAParenthesis = false;
                _op_stack.Add(tokens[i]);
                continue;
            }

            if (tokens[i] == ")")
            {
                hasReachedAParenthesis = false;
                hasExitAParenthesis = true;
            }

            if ((hasReachedAParenthesis && inputOperatorValue > operatorInStackValue) ||
                inputOperatorValue > operatorInStackValue)
            {
                _op_stack.Add(tokens[i]);
                continue;
            }

            while (inputOperatorValue <= operatorInStackValue &&
                ((hasReachedAParenthesis || hasExitAParenthesis) || (!hasExitAParenthesis && _op_stack.Count > 0)))
            {
                if (hasExitAParenthesis && _op_stack[counter] == "(")
                {
                    _op_stack.Remove(_op_stack[counter]);
                    counter--;
                }
                else if (!hasExitAParenthesis && _op_stack[counter] == "(") break;

                _output.Add(_op_stack[counter]);
                _op_stack.RemoveAt(counter);

                if (_op_stack.Count == 0) break;
                else counter--;
            }

            if (counter != 0)
            {
                if (_op_stack[counter] == "(" && _op_stack.Count != 0)
                    _op_stack.Add(tokens[i]);
            }
            if (_op_stack.Count == 0 && tokens[i] != ")") _op_stack.Add(tokens[i]);
        }

        while (_op_stack.Count > 0)
        {
            _output.Add(_op_stack.Last<String>());
            _op_stack.RemoveAt(_op_stack.Count - 1);
        }

        foreach (var item in _output)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        SolvingOutput(_output);
        _output.Clear();
    }

    public void CheckingValidations(string input)
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

        ParsingOperations(tokens);
        
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

                case Actions.POWER:
                    VariablePostCalculations = CalculatorOperations.RaiseToThePower(firstNumber, secondNumber);
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
        bool operatorHasCorrectPosition = false;
        bool numberHasCorrectPosition = false;
        bool hasReachedAParenthesis = false;
        bool hasExitAParenthesis = false;

        if (double.TryParse(arr[0], out _)) RestartValues();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == "(")
            {
                hasReachedAParenthesis = true;
                hasExitAParenthesis = false;
                continue;
            }
            else if (arr[i] == ")")
            {
                hasExitAParenthesis = true;
                hasReachedAParenthesis = false;
                continue;
            }

            if (VariablePostCalculations.HasValue && arr.Length > 1) 
            {
                operatorHasCorrectPosition = MyActions.Contains(arr[i].ToUpper());
                numberHasCorrectPosition = i % 2 != 0 && double.TryParse(arr[i], out _);

                if (hasReachedAParenthesis)
                {
                    operatorHasCorrectPosition = i % 2 != 0 && MyActions.Contains(arr[i].ToUpper());
                    numberHasCorrectPosition = double.TryParse(arr[i], out _);
                }

                if (hasExitAParenthesis)
                {
                    operatorHasCorrectPosition = MyActions.Contains(arr[i].ToUpper());
                    numberHasCorrectPosition = i % 2 != 0 && double.TryParse(arr[i], out _);
                }
            }

            if (!VariablePostCalculations.HasValue)
            {
                numberHasCorrectPosition = double.TryParse(arr[i], out _);
                operatorHasCorrectPosition = i % 2 != 0 && MyActions.Contains(arr[i].ToUpper());

                if (hasReachedAParenthesis)
                {
                    numberHasCorrectPosition = i % 2 != 0 && double.TryParse(arr[i], out _);
                    operatorHasCorrectPosition = MyActions.Contains(arr[i].ToUpper());
                }

                if (hasExitAParenthesis)
                {
                    numberHasCorrectPosition = double.TryParse(arr[i], out _);
                    operatorHasCorrectPosition = i % 2 != 0 && MyActions.Contains(arr[i].ToUpper());
                }
            }

            if (!numberHasCorrectPosition && !operatorHasCorrectPosition) return false;

            if (operatorHasCorrectPosition) countingOperations++;
            if (operatorHasCorrectPosition && (countingOperations < 1 || countingOperations > _maxConsecutivesOperations)) return false;
        }
        return true;
    }

}