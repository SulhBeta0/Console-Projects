using System.Runtime.Versioning;
using Clicks.Modules;
using Clicks.Tools;
namespace Clicks;

[SupportedOSPlatform("windows")]
public static class Menus
{
    public static void UpdateMenu()
    {
        char letter;
        bool cheking;
        do
        {
            StartingMenu();
            letter = Console.ReadKey(true).KeyChar;
            cheking = CheckIfExit(letter);
            
            Thread.Sleep(60);
            Player.MyAction(letter);
        }
        while (!cheking);
    }

    public static void UpgradesMenu()
    {
        char buy;
        bool checking = false;
        while(!checking)
        {
            Accesories.MessageCreator("-|UPGRADES|-", ConsoleColor.White, 5, null);

            Accesories.MessageCreator(
            $"-{Click.Instance.LevelUpgrade}Lv- Power: {Sufixs.GettingNewLook(Click.Instance.Power)} ===> {Sufixs.GettingNewLook(Click.GetNewUpgradeValue())} ({UpgradeMenuAdjustment("click")}) €",
            ConsoleColor.Blue, 10, 43);

            Accesories.MessageCreator(
                $"-{Experience.Instance.LevelUpgrade}Lv- XP/click: {Experience.Instance.XpToGainPerClick} ===> {Experience.GetNewUpgradeValue()} ({UpgradeMenuAdjustment("experience")}) €",
                ConsoleColor.Blue, 12, 43);

            Accesories.MessageCreator(
                $"-{Money.Instance.LevelUpgrade}Lv- Money multiplier: x{Money.Instance.Multiplier} ===> x{Money.GetNextUpgradeValue()} ({UpgradeMenuAdjustment("money")}) €",
                ConsoleColor.Blue, 14, 43);

            Accesories.MessageCreator(
                $"-{CriticalChance.Instance.LevelUpgrade}Lv- Critical chance: {CriticalChance.Instance.Chance / 100:P0} ===> {CriticalChance.GetNewUpgradeValue() / 100:P0} ({UpgradeMenuAdjustment("critics")}) €",
                ConsoleColor.Blue, 16, 43);

            Accesories.MessageCreator("--ESC to exit--",
               ConsoleColor.DarkGray, 20, null);

            buy = Console.ReadKey(true).KeyChar;
            checking = CheckIfExit(buy);

            if (!checking) Player.BuyUpgrade(buy);
        }
    }

    public static void StatsMenu()
    {
        Accesories.MessageCreator("--STATS--", ConsoleColor.White,8, null);

        Accesories.MessageCreator($"- Total clicks => {Player.Instance.TotalClicksMade}",
            ConsoleColor.DarkYellow,
            11, 49);

        Accesories.MessageCreator($"- My power => {Click.Instance.Power}/click",
            ConsoleColor.DarkYellow,
            13, 49);
        
        Accesories.MessageCreator($"- Money multiplier => x{Money.Instance.Multiplier:F1}",
            ConsoleColor.DarkYellow,
            15, 49);

        Accesories.MessageCreator($"- Critic chance => {CriticalChance.Instance.Chance/100:P0}",
            ConsoleColor.DarkYellow,
            17, 49);
    }

    public static void PreLoadMenu()
    {
        if (!File.Exists(Accesories.filePath))
        {
            Accesories.MessageCreator("NEW VISITOR :D!!!", ConsoleColor.Cyan, 15, null);
            Thread.Sleep(1300);
            return;
        }

        SavingPlayer.Load();
        LoadingAnim();
    }

    public static void AccesingModules(Action menu)
    {
        Console.Clear();
        menu();

        Console.ReadKey(true);
        Console.Clear();
    }

    private static void LoadingAnim()
    {
        char[] anim = ['▁', '▂', '▃', '▄', '▅', '▆', '▇', '█'];

        for (int i = 0; i < anim.Length; i++)
        {
            Accesories.MessageCreator($"Loading {anim[i]}", ConsoleColor.White, 15, null);
            Thread.Sleep(450);
        }
    }

    private static bool CheckIfExit(char button)
    {
        if ((ConsoleKey)button == ConsoleKey.Escape) { return true; }

        return false;
    }

    private static void StartingMenu()
    {
        Accesories.MessageCreator
            (
            $"CLICKS: [{Sufixs.GettingNewLook(Player.Instance.ConsecutiveClicks)}] | " +
            $"MONEY: [{Sufixs.GettingNewLook((ulong)Player.Instance.Balance)}€] | " +
            $"LEVEL: [{Player.Instance.Level}] | " +
            $"XP: [{Sufixs.GettingNewLook((ulong)Player.Instance.XP)}] | " +
            $"NEXT XP [{Sufixs.GettingNewLook((ulong)Experience.Instance.NextAmountXpLevelUp)}]",
            ConsoleColor.DarkCyan, 2, null
            );

        Accesories.MessageCreator
            ("--Spacebar to \"click\"--", ConsoleColor.DarkGray, 6, null );

        Accesories.MessageCreator
            ("--ESC to exit--", ConsoleColor.DarkGray, 7, null);

        AccesiblePartsInStartZone();

        CriticalChance.Instance.CalculatingCritics();
    }

    private static string UpgradeMenuAdjustment(string upgradeName)
    {
        switch (upgradeName)
        {
            case "click":
                if (Click.Instance.LevelUpgrade >= Click.Instance.MaxUpgrades) return "MAX REACHED!";
                else return Sufixs.GettingNewLook((ulong)Click.Instance.NextUpgradeCost);

            case "money":
                if (Money.Instance.LevelUpgrade >= Money.Instance.MaxUpgrades) return "MAX REACHED!";
                else return Sufixs.GettingNewLook((ulong)Money.Instance.NextUpgradeCost);

            case "experience":
                if (Experience.Instance.LevelUpgrade >= Experience.Instance.MaxUpgrades) return "MAX REACHED!";
                else return Sufixs.GettingNewLook((ulong)Experience.Instance.NextUpgradeCost);

            case "critics":
                if (CriticalChance.Instance.LevelUpgrade >= CriticalChance.Instance.MaxUpgrades) return "MAX REACHED!";
                else return Sufixs.GettingNewLook((ulong)CriticalChance.Instance.NextUpgradeCost);
        }

        return "ERROR";
    }

    private static void AccesiblePartsInStartZone()
    {
        Accesories.MessageCreator("[1] | UPGRADES", ConsoleColor.White, 15, 53);
        Accesories.MessageCreator("[2] | SELL CLICKS", ConsoleColor.White, 16, 53);
        Accesories.MessageCreator("[3] | PROFILE", ConsoleColor.White, 17, 53);
        Accesories.MessageCreator("[4] | SAVE", ConsoleColor.White, 18, 53);
    }
}
