using System.Runtime.Versioning;
using Clicks.Tools;
namespace Clicks.UpgradZone;

[SupportedOSPlatform("windows")]
public abstract class UpgradeBases
{
    public abstract string Name { get; }
    public abstract double BaseCost { get; }
    public abstract byte MaxUpgrades { get; }
    public byte LevelUpgrade { get; protected set; }
    public double NextUpgradeCost { get; protected set; }

    public abstract void CalculateNextUpgradeCost();
    public abstract void NextUpgradeStat();
    
    public void ApplyUpgrade()
    {
        if (LevelUpgrade <= MaxUpgrades)
        {
            NextUpgradeStat();
            CalculateNextUpgradeCost();
            LevelUpgrade++;
        }
        else { Accesories.PathForMusic("Clicks.Tools.Sounds.error.wav", 40); }
    }
}
