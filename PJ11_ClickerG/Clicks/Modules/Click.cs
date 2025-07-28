using System.Runtime.Versioning;
using Clicks.UpgradZone;
namespace Clicks.Modules;

[SupportedOSPlatform("windows")]
public class Click : UpgradeBases
{
    private static readonly Click _instance = new();

    public static Click Instance => _instance ?? new Click();
    public ulong Power { get; private set; }
    public static double ClickValue => Money.CalculatingValue();

    public override string Name => "Click";
    public override double BaseCost => 160.00;
    public override byte MaxUpgrades => 25;

    private Click()
    {
        Power = 1;
        LevelUpgrade = 1;
        NextUpgradeCost = BaseCost;
    }

    public override void CalculateNextUpgradeCost() => NextUpgradeCost = BaseCost * Math.Pow(4.5, LevelUpgrade);
    public override void NextUpgradeStat() => Power += Power;
    public static ulong GetNewUpgradeValue() => Instance.Power * 2;

    public static bool CanClick() => Player.Instance.ConsecutiveClicks < ulong.MaxValue - 1;

    public void ClicksNewData(BinaryReader data)
    {
        Power = data.ReadUInt64();
        LevelUpgrade = data.ReadByte();
    }
}

