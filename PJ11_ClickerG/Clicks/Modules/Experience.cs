using System.Runtime.Versioning;
using Clicks.Tools;
using Clicks.UpgradZone;
namespace Clicks.Modules;

[SupportedOSPlatform("windows")]
public class Experience : UpgradeBases
{
    private static readonly Experience _instance = new();
    private const float _upgradeValue = 0.50f;
    
    public static Experience Instance => _instance ?? new Experience();
    public double XpToGainPerClick { get; private set; }
    public double NextAmountXpLevelUp { get; private set; }
    
    public const byte maxLevel = 100;

    public override string Name => "XP";
    public override double BaseCost => 1200.00;
    public override byte MaxUpgrades => 13;

    private Experience()
    {
        XpToGainPerClick = 0.35;
        NextAmountXpLevelUp = 100.00;
        LevelUpgrade = 1;
        NextUpgradeCost = BaseCost;
    }

    public override void CalculateNextUpgradeCost() => NextUpgradeCost = BaseCost * Math.Pow(6, LevelUpgrade);
    public override void NextUpgradeStat() => XpToGainPerClick += _upgradeValue;
    public static double GetNewUpgradeValue() => Instance.XpToGainPerClick + _upgradeValue;

    public void NewXpToLevelUp()
    {
        Player.Instance.NewLevel();
        Accesories.PathForMusic("Clicks.Tools.Sounds.level.wav", 650);
        NextAmountXpLevelUp = CalculateNewXpToLevel();

        Console.Clear();
    }

    public void ExperienceNewData(BinaryReader data)
    {
        XpToGainPerClick = data.ReadDouble();
        NextAmountXpLevelUp = data.ReadDouble();
        Instance.LevelUpgrade = data.ReadByte();
    }

    private double CalculateNewXpToLevel() => NextAmountXpLevelUp * 1.50;
}

