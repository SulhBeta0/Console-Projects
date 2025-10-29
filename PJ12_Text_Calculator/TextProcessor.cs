namespace PJ12_Text_Calculator;

internal static class TextProcessor
{

    internal static string Text()
    {
        Messages("<<help:\n", ConsoleColor.DarkGray);
        Messages("C://Users//Calculator.exe> ", ConsoleColor.DarkGreen);

        string text = Console.ReadLine()?.ToUpper()!;

        //string secureText = String.IsNullOrWhiteSpace(text) ? " " : text;
        return text;
    }

    internal static void Commands(string[] actions)
    {
        Console.Clear();
        Messages("=> /" + String.Join(" /", actions) + "\n", ConsoleColor.White);
    }

    internal static void Messages(string myText, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(myText);
        Console.ResetColor();
    }
}
