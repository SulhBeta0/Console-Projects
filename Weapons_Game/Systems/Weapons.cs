
namespace Weapons_Game.Systems
{
    class Weapons
    {
        private readonly PlayerData _data;

        public int Damage { get; private set; }
        public double Cooldown { get; private set; }
        public static readonly float probabilityBetterDamage = 0.15f; //15% all weapons

        public Dictionary<string, (int damage, double cooldown, double price)> SharpStats { get; } = new()
        {
            [Sharp.GAUNTLET.ToString()] = (8, 0.44, 0.0),
            [Sharp.KNIFE.ToString()] = (34, 1.05, 500.0),
            [Sharp.SICKLE.ToString()] = (25, 0.52, 1_150.00),
            [Sharp.KATANA.ToString()] = (65, 1.20, 3_455.00),
            [Sharp.SWORD.ToString()] = (90, 1.5, 7_500.00),
            [Sharp.AXE.ToString()] = (190, 3.0, 13_000.99),
            [Sharp.SCYTHE.ToString()] = (370, 1.88, 29_999.99)
        };//Can add more
        public enum Sharp
        {
            SICKLE /*Hoz*/,
            KNIFE,
            GAUNTLET,
            KATANA,
            SCYTHE /*Guadaña*/,
            AXE,
            SWORD
        }

        public Weapons(PlayerData data) => _data = data;

        public int SetDamage() => Damage = SharpStats[_data.Weapon.ToString()].damage;
        public double SetCooldown() => Cooldown = SharpStats[_data.Weapon.ToString()].cooldown;
    }
}
