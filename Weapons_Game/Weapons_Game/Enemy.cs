using Weapons_Game.database;
using Weapons_Game.Systems;

namespace Weapons_Game
{
    class Enemy
    {
        private readonly PlayerData _data;

        public int Health { get; private set; }

        public enum Estados { LV1, LV2 }

        public Enemy(PlayerData data) => _data = data;

        public void SetHealth(int health) => Health = health;
        
        public static (string name, int health) NewEnemy()
        {
            try {
                using var conexion = MyDatabase.GetConnection();
                return MyDatabase.GettingEnemyStats(conexion);
            }
            catch (Exception)
            {
                Console.WriteLine(Tools.WarningsCreator("THIS ERROR IS YOUR ENEMY"));
                return (String.Empty, 0);
            }
        }

        public static (string Name, byte Quantity) DropSystem(string nombreEnemigo)
        {
            try {
                using var conexion = MyDatabase.GetConnection();
                return MyDatabase.GettingEnemyDrops(conexion, nombreEnemigo);
            }
            catch (Exception)
            {
                Console.WriteLine(Tools.WarningsCreator("THIS ERROR IS YOUR ENEMY"));
                return (String.Empty, 0);
            }
        }

        public static double SellValuesDrops(string item)
        {
            try {
                using var conexion = MyDatabase.GetConnection();
                return MyDatabase.GettingDropValue(conexion, item);
            }
            catch (Exception)
            {
                Console.WriteLine(Tools.WarningsCreator("THIS ERROR IS YOUR ENEMY"));
                return (0.00);
            }
        }
    }
}
