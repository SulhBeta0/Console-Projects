//Finished on July 28th, 2025
//Tap to earn money, buy upgrades and level up.

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
