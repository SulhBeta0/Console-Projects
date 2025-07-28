using System.Runtime.Versioning;
using Clicks.Modules;
using Clicks.Tools;
namespace Clicks;

[SupportedOSPlatform("windows")]
public class Player
{
    private static readonly Player _instance = new();

    public static Player Instance => _instance ?? new Player();
    public int Level { get; private set; }
    public ulong ConsecutiveClicks { get; private set; }
    public ulong TotalClicksMade { get; private set; }
    public double Balance { get; private set; }
    public double XP { get; private set; }

    private Player()
    { 
        Level = 0;
        ConsecutiveClicks = 0;
        TotalClicksMade = 0;
        Balance = 0.0;
        XP = 0.0;
    }

    public void NewLevel() => Level++;

    public static void MyAction(char button)
    {
        Accesories.ClearKeyBuffer();
        if ((ConsoleKey)button == ConsoleKey.Spacebar && Click.CanClick())
        {
            Instance.AddingClicks();
            return;
        }

        switch (button.ToString())
        {
            case "1":
                Accesories.PathForMusic("Clicks.Tools.Sounds.click.wav", 50);
                Menus.AccesingModules(Menus.UpgradesMenu);
                return;

            case "2":
                Instance.SellClicks();
                return;

            case "3":
                Accesories.PathForMusic("Clicks.Tools.Sounds.click.wav", 50);
                Menus.AccesingModules(Menus.StatsMenu);
                return;

            case "4":
                SavingPlayer.Instance.Save();
                return;

            default:
                return;
        }
    }

    public static void BuyUpgrade(char myAction)
    {
        switch (myAction.ToString())
        {
            case "1" when CheckingBalanceToBuy(Click.Instance.NextUpgradeCost):
                Instance.Balance -= Click.Instance.NextUpgradeCost;
                Click.Instance.ApplyUpgrade();
                return;

            case "2" when CheckingBalanceToBuy(Experience.Instance.NextUpgradeCost):
                Instance.Balance -= Experience.Instance.NextUpgradeCost;
                Experience.Instance.ApplyUpgrade();
                return;

            case "3" when CheckingBalanceToBuy(Money.Instance.NextUpgradeCost):
                Instance.Balance -= Money.Instance.NextUpgradeCost;
                Money.Instance.ApplyUpgrade();
                return;

            case "4" when CheckingBalanceToBuy(CriticalChance.Instance.NextUpgradeCost):
                Instance.Balance -= CriticalChance.Instance.NextUpgradeCost;
                CriticalChance.Instance.ApplyUpgrade();
                return;

            default:
                Accesories.PathForMusic("Clicks.Tools.Sounds.error.wav", 40);
                return;

        }
    }

    public void PlayerNewData(BinaryReader data)
    {
        Level = data.ReadInt32();
        ConsecutiveClicks = data.ReadUInt64();
        TotalClicksMade = data.ReadUInt64();
        Balance = data.ReadDouble();
        XP = data.ReadDouble();
    }

    private static bool CheckingBalanceToBuy(double costUpgrade) => Instance.Balance >= costUpgrade;

    private void SellClicks()
    {
        if (ConsecutiveClicks > 0)
        {
            Accesories.PathForMusic("Clicks.Tools.Sounds.retro.wav", 50);
            Balance += Click.ClickValue;
            ConsecutiveClicks -= ConsecutiveClicks;
        }
    }

    private void AddingClicks()
    {
        ulong calc = Click.Instance.Power * CriticalChance.Instance.CalculatingCritics();

        ConsecutiveClicks += calc;
        TotalClicksMade++;

        if (Level < Experience.maxLevel)
        {
            XP += Experience.Instance.XpToGainPerClick;
            if (XP >= Experience.Instance.NextAmountXpLevelUp)
            {
                XP = 0.0;
                Experience.Instance.NewXpToLevelUp();
                return;
            }
        }
    }
}
