//Finished on May 12th, 2025
//Simple game where you kill enemies that drop things so you can sell them and buy more weapons.

using Weapons_Game;
using Weapons_Game.Systems;

class Program
{
    public static void Main()
    {
        //MyDatabase.Init(); //To create the database once

        Tools.MessageCreator("- Enter your name -");
        string myName = Console.ReadLine()!.ToUpper();

        PlayerData playerData = new(myName);
        Player myPlayer = new(playerData);
        Weapons weapons = new(playerData);
        Enemy enemys = new(playerData);
        Game_Systems games = new(myPlayer, playerData, weapons, enemys);

        byte input;
        Console.Clear();
        do {
            Tools.MessageCreator("-- SYSTEM --");
            Tools.MessageCreator("1.Resources\n2.Shop\n3.Profile\n4.Change [Weapon]\n5.Save [Data]\n6.Load [Data]\n7.Exit...", ConsoleColor.Yellow);
            Console.ResetColor();

            Console.Write("\n-> ");
            if (byte.TryParse(Console.ReadLine(), out input))
            {
                myPlayer.Write(input, games);
            }
            else {
                Console.WriteLine(Tools.WarningsCreator("WRONG TYPE OF INPUT!"));
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        while (input != 7);
    }
}
