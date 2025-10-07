//Finished on ??? 0th, 2025 -

using System.Runtime.Versioning;
namespace PJ12_Text_Calculator;

public class Program
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("HAY ERRORES :v\nDE MOMENTO NO HAY JERARQUÍA EN OPERACIONES ASI QUE LOS CALCULOS PODRÁN ESTAR MAL SEGUN QUE OPERACIONES SE HAGA\n");
        Console.ResetColor();

        Calculator.Update();
        Environment.Exit(0);
    }
}