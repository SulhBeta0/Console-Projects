using System.Runtime.Versioning;
using Clicks.Modules;
namespace Clicks.Tools;

[SupportedOSPlatform("windows")]
public class SavingPlayer
{
    private static readonly SavingPlayer _instance = new();

    public static SavingPlayer Instance => _instance ?? new SavingPlayer();
    public int MyLevel { get; set; }
    public ulong MyConsecutiveClicks { get; set; }
    public ulong MyTotalClicksMade { get; set; }
    public double MyMoney { get; set; }
    public double MyXP { get; set; }

    //Experience
    public double MyXpToGainPerClick { get; set; }
    public double MyNextAmountXpLevelUp { get; set; }
    public byte LevelUpgradeExperience { get; set; }

    //Critics
    public double MyCriticalClickChance { get; set; }
    public byte LevelUpgradeCritics { get; set; }

    //Money
    public double MyMoneyMultiplier { get; set; }
    public byte LevelUpgradeMoney { get; set; }

    //Clicks
    public ulong MyClickPower { get; set; }
    public byte LevelUpgradeClicks { get; set; }

    private SavingPlayer()
    {
        MyLevel = 0;
        MyConsecutiveClicks = 0;
        MyTotalClicksMade = 0;
        MyMoney = 0.0;
        MyXP = 0.0;

        MyXpToGainPerClick = 0.5;
        MyNextAmountXpLevelUp = 100.00;
        LevelUpgradeExperience = 0;

        MyCriticalClickChance = 0.0;
        LevelUpgradeCritics = 0;

        MyMoneyMultiplier = 1.0;
        LevelUpgradeMoney = 0;

        MyClickPower = 1;
        LevelUpgradeClicks = 0;
    }

    public void Save()
    {
        MyLevel = Player.Instance.Level;
        MyConsecutiveClicks = Player.Instance.ConsecutiveClicks;
        MyTotalClicksMade = Player.Instance.TotalClicksMade;
        MyMoney = Player.Instance.Balance;
        MyXP = Player.Instance.XP;

        MyXpToGainPerClick = Experience.Instance.XpToGainPerClick;
        MyNextAmountXpLevelUp = Experience.Instance.NextAmountXpLevelUp;
        LevelUpgradeExperience = Experience.Instance.LevelUpgrade;

        MyCriticalClickChance = CriticalChance.Instance.Chance;
        LevelUpgradeCritics = CriticalChance.Instance.LevelUpgrade;

        MyMoneyMultiplier = Money.Instance.Multiplier;
        LevelUpgradeMoney = Money.Instance.LevelUpgrade;

        MyClickPower = Click.Instance.Power;
        LevelUpgradeClicks = Click.Instance.LevelUpgrade;

        try {
            using FileStream option = new(Accesories.filePath, FileMode.Create);
            using BinaryWriter dataWriter = new(option);

            dataWriter.Write(MyLevel);
            dataWriter.Write(MyConsecutiveClicks);
            dataWriter.Write(MyTotalClicksMade);
            dataWriter.Write(MyMoney);
            dataWriter.Write(MyXP);

            dataWriter.Write(MyXpToGainPerClick);
            dataWriter.Write(MyNextAmountXpLevelUp);
            dataWriter.Write(LevelUpgradeExperience);

            dataWriter.Write(MyCriticalClickChance);
            dataWriter.Write(LevelUpgradeCritics);

            dataWriter.Write(MyMoneyMultiplier);
            dataWriter.Write(LevelUpgradeMoney);

            dataWriter.Write(MyClickPower);
            dataWriter.Write(LevelUpgradeClicks);

            Accesories.PathForMusic("Clicks.Tools.Sounds.notify.wav", 50);
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine(Accesories.WarningsCreator
                (
                "ERROR: YOUR FILE HAS NOT BEEN SAVED",
                15, null)
                );
        }
    }

    public static void Load()
    {
        try {
            if (File.Exists(Accesories.filePath))
            {
                using FileStream option = new(Accesories.filePath, FileMode.Open);
                using BinaryReader dataReader = new(option);

                Player.Instance.PlayerNewData(dataReader);
                Experience.Instance.ExperienceNewData(dataReader);
                CriticalChance.Instance.CriticsNewData(dataReader);
                Money.Instance.MoneyMultiplierNewData(dataReader);
                Click.Instance.ClicksNewData(dataReader);
            }
            else
            {
                Console.WriteLine(Accesories.WarningsCreator(
                    "ERROR: YOUR FILE DO NOT EXIST",
                    25, null));
            }
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine(Accesories.WarningsCreator(
                "AN ERROR HAS APPEARD. STOPPED LOADING PROCESS",
                15, null));
        }
    }
}
