
namespace Weapons_Game.Systems
{
    class SavingStructure
    {
        public string MyName { get; set; } = "Default_00000";
        public double MyMoney { get; set; } = 300.00;
        public uint MyKillCount { get; set; } = 0;
        public bool MyMessageForNewState { get; set; } = false;
        public Weapons.Sharp MyWeapon { get; set; } = Weapons.Sharp.GAUNTLET;
        public Enemy.Estados MyLevelState { get; set; } = Enemy.Estados.LV1;
        public Dictionary<string, uint> MyInventoryResources { get; set; } = new();
        public List<string> MyInventoryWeapons { get; set; } = new() { Weapons.Sharp.GAUNTLET.ToString() };
    }
}
