//Finished on -
//Tap to earn money, buy upgrades and level up.

// Me queda - 
// Hacer la lógica de los logros
// Modificar los menús segun: Que logros o mejoras tengo y si he alcanzado el maximo de alguna 

using System.Runtime.Versioning;
using Clicks.Tools;
namespace Clicks;

[SupportedOSPlatform("windows")]
class Program
{
    static void Main()
    {
        Accesories.SetUpConsole(false, "Clicker");
        Accesories.ChangingWindowsValues(60);

        Menus.PreLoadMenu();
        Console.Clear();
        Menus.UpdateMenu();

        Environment.Exit(0);
    }
}
