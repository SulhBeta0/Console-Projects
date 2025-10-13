namespace PJ12_Text_Calculator;

#pragma warning disable CA1416
internal static class TextProcessor
{
    internal static string Text()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Escriba --COMMANDS-- como ayuda => ");

        string text = Console.ReadLine()!.ToUpper()!;
        Console.ResetColor();

        string secureText = String.IsNullOrWhiteSpace(text) ? "0" : text;
        return secureText;
    }

    internal static (List<double> numbers, List<string> operations) SplitText
        (string myText, double? acumulator, List<double> listNumbers, List<string> listOperations)
    {
        int countOperations = 0;

        //string[] textSortedAndSplited = SortingText(myText).Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] textSplit = myText.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (!acumulator.HasValue && !double.TryParse(textSplit[0], out _))
        {
            Console.Beep(450, 300);
            return (new List<double>(), new List<string>());
        }

        for (int i = 0; i < textSplit.Length; i++)
        {
            if (double.TryParse(textSplit[i], out double validNum))
            {
                listNumbers.Add(validNum);
                continue;
            }

            if (Calculator.MyActions.Contains<string>(textSplit[i]) &&
                countOperations <= Calculator._maxConsecutivesOperations)
            {
                listOperations.Add(textSplit[i]);
                countOperations++;
            }
        }

        if (listOperations.Count < 1 || listOperations.Count > Calculator._maxConsecutivesOperations)
        {
            Console.Beep(450, 300);
            return (new List<double>(), new List<string>());
        }

        return (numbers: listNumbers, operations: listOperations);
    }

    internal static void Commands()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("=> " + String.Join(" | ", Calculator.MyActions) + "\n");
        Console.ResetColor();
    }

    static string SortingText(string textToSort)
    {
        return textToSort;
    }
}
