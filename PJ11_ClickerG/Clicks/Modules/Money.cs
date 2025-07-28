using System.Runtime.Versioning;
using Clicks.UpgradZone;
namespace Clicks.Modules;

[SupportedOSPlatform("windows")]
public class Money : UpgradeBases
{
    private static readonly Money _instance = new();
    private const float _upgradeValue = 0.50f;

    public static Money Instance => _instance ?? new Money();
    public double Multiplier { get; private set; }

    public override string Name => "Money Multiplier";
    public override double BaseCost => 145.00;
    public override byte MaxUpgrades => 15;

    private Money()
    {
        Multiplier = 1.0;
        LevelUpgrade = 1;
        NextUpgradeCost = BaseCost;
    }

    public override void CalculateNextUpgradeCost() => NextUpgradeCost = BaseCost * Math.Pow(5, LevelUpgrade);
    public override void NextUpgradeStat() => Multiplier += _upgradeValue;
    public static double CalculatingValue() => Player.Instance.ConsecutiveClicks * Instance.Multiplier;
    public static double GetNextUpgradeValue() => Instance.Multiplier + _upgradeValue;

    public void MoneyMultiplierNewData(BinaryReader data)
    {
        Multiplier = data.ReadDouble();
        LevelUpgrade = data.ReadByte();
    }
}
