using System.Runtime.Versioning;
using Clicks.UpgradZone;
namespace Clicks.Modules;

[SupportedOSPlatform("windows")]
public class CriticalChance : UpgradeBases
{
    private static readonly CriticalChance _instance = new();
    private static readonly Random _rng = new();
    private const float _upgradeValue = 1.0f;

    public static CriticalChance Instance => _instance ?? new CriticalChance();
    
    public double Chance { get; private set; }

    public override string Name => "Critics";
    public override double BaseCost => 500.00;
    public override byte MaxUpgrades => 37;
    
    private CriticalChance()
    {
        Chance = 0.0;
        LevelUpgrade = 1;
        NextUpgradeCost = BaseCost;
    }

    public override void CalculateNextUpgradeCost() => NextUpgradeCost = BaseCost * Math.Pow(3.7, LevelUpgrade);
    public override void NextUpgradeStat() => Chance += _upgradeValue;
    public static double GetNewUpgradeValue() => Instance.Chance + _upgradeValue;

    public void CriticsNewData(BinaryReader data)
    {
        Chance = data.ReadDouble();
        LevelUpgrade = data.ReadByte();
    }

    public byte CalculatingCritics()
    {
        double chance = _rng.NextDouble();

        if (LevelUpgrade > 0 && chance <= Chance/100) return 2;
        else return 1;
    }
}

