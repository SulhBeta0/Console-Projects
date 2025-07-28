using Microsoft.Data.Sqlite;
using Weapons_Game.Systems;

namespace Weapons_Game.database
{
    public static class MyDatabase
    {
        private static readonly Random _rng = new();
        private static readonly string _stringConnection = @$"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "base_Datos.db")}";
        private static SqliteConnection? _conectarBase;

        static MyDatabase() => _conectarBase = new SqliteConnection(_stringConnection);

        //public static void Init()
        //{
        //    if (!File.Exists(_stringConnection))
        //    {
        //        using var conectar = new SqliteConnection(_stringConnection);
        //        conectar.Open();

        //        string queryEnemyTable = @"CREATE TABLE IF NOT EXISTS Enemy
        //        (
        //                State TEXT NOT NULL,
        //                Name TEXT NOT NULL,
        //                Health INTEGER NOT NULL,
        //                PRIMARY KEY (Name)
        //        );";

        //        string queryEnemyDropsTable = @"CREATE TABLE IF NOT EXISTS EnemyDrops
        //        (
        //                EnemyName TEXT NOT NULL,
        //                Drops TEXT NOT NULL,
        //                ProbabilityToDropItem REAL NOT NULL,
        //                MinItemsToDrop INTEGER NOT NULL,
        //                MaxItemsToDrop INTEGER NOT NULL,
        //                ValueDrop REAL NOT NULL,
        //                FOREIGN KEY (EnemyName) REFERENCES Enemy(Name)
        //        );";

        //        using var comando = conectar.CreateCommand();
        //        comando.CommandText = queryEnemyTable;
        //        comando.ExecuteNonQuery();

        //        comando.CommandText = queryEnemyDropsTable;
        //        comando.ExecuteNonQuery();
        //    }
        //}

        public static void InsertDropsInit()
        {
            using var conexion = new SqliteConnection(_stringConnection);
            conexion.Open();

            using var funcPragma = conexion.CreateCommand();
            funcPragma.CommandText = "PRAGMA foreign_keys = ON;";
            funcPragma.ExecuteNonQuery();

            using var transaccion = conexion.BeginTransaction();

            try
            {
                InsertDrops(conexion, "COW", "cow_meat", 60.0f, 1, 7, 15.0);
                InsertDrops(conexion, "COW", "leather", 40.0f, 1, 4, 40.0);

                InsertDrops(conexion, "CHICKEN", "chicken_meat", 70.0f, 1, 5, 15.50);
                InsertDrops(conexion, "CHICKEN", "feather", 29.0f, 2, 5, 30.0);
                InsertDrops(conexion, "CHICKEN", "egg", 0.99f, 2, 4, 60.0);
                InsertDrops(conexion, "CHICKEN", "golden_egg", 0.01f, 1, 2, 500.0);

                InsertDrops(conexion, "PIG", "pig_meat", 95.0f, 1, 3, 50.0);
                InsertDrops(conexion, "PIG", "head", 5.0f, 1, 2, 90.50);

                InsertDrops(conexion, "SPIDER", "spider_meat", 80.0f, 1, 2, 65.0);
                InsertDrops(conexion, "SPIDER", "leg", 17.0f, 2, 6, 100.0);
                InsertDrops(conexion, "SPIDER", "eye", 3.0f, 1, 2, 135.0);

                InsertDrops(conexion, "KNIGHT", "potion", 67.0f, 1, 3, 100.0);
                InsertDrops(conexion, "KNIGHT", "helmet", 22.0f, 1, 2, 200.0);
                InsertDrops(conexion, "KNIGHT", "chestplate", 8.50f, 1, 2, 250.0);
                InsertDrops(conexion, "KNIGHT", "sword", 2.50f, 1, 2, 670.0);

                InsertDrops(conexion, "SOLDIER", "clothes", 75.50f, 1, 5, 299.99);
                InsertDrops(conexion, "SOLDIER", "rifle", 17.0f, 1, 2, 550.00);
                InsertDrops(conexion, "SOLDIER", "ammo", 7.50f, 10, 100, 35.50);

                InsertDrops(conexion, "ORC", "tooth", 99.0f, 2, 7, 350.0);
                InsertDrops(conexion, "ORC", "mace", 0.999f, 1, 2, 1_234.56);
                InsertDrops(conexion, "ORC", "ring", 0.001f, 1, 2, 13_450.0);

                InsertDrops(conexion, "DECEPTICON", "metal", 77.0f, 5, 10, 300.0);
                InsertDrops(conexion, "DECEPTICON", "gasoline", 22.99999f, 1, 3, 850.0);
                InsertDrops(conexion, "DECEPTICON", "optimus_sword", 0.00001f, 1, 2, 60_000.0);
                transaccion.Commit();
            }
            catch (Exception) { transaccion.Rollback(); }
        }

        //public static void AddingAllTypeChanges() //In case I want to add things
        //{
        //    using var conexion = new SqliteConnection(_stringConnection);
        //    conexion.Open();

        //    using var comando = conexion.CreateCommand();
        //    comando.CommandText = @"";

        //    comando.ExecuteNonQuery();
        //}

        public static (string enemyName, int enemyHealth) GettingEnemyStats(SqliteConnection conexion)
        {
            string name = String.Empty;
            int health = 0, countRows = 0;

            using var comando = conexion.CreateCommand();
            switch (PlayerData.LevelState)
            {
                case Enemy.Estados.LV1:
                    comando.CommandText = "SELECT COUNT(State) FROM Enemy WHERE State = 'LV1'";
                    break;

                case Enemy.Estados.LV2:
                    comando.CommandText = "SELECT COUNT(State) FROM Enemy WHERE State IN ('LV1', 'LV2')";
                    break;
                default:
                    Tools.WarningsCreator("UNRECOGNIZED STATE");
                    countRows = 0;
                    return (String.Empty, 0);
            }
            countRows = Convert.ToInt32(comando.ExecuteScalar());

            comando.CommandText = "SELECT Name, Health FROM Enemy WHERE Id = @Number";
            comando.Parameters.AddWithValue("@Number", _rng.Next(1, countRows + 1));

            using var dataReader = comando.ExecuteReader();
            if (dataReader.Read())
            {
                name = dataReader.GetString(0);
                health = int.Parse(dataReader.GetString(1));
            }

            return (enemyName: name, enemyHealth: health);
        }

        public static (string enemyDropName, byte dropsQuantity) GettingEnemyDrops(SqliteConnection conexion, string enemyName)
        {
            string nombreItem = String.Empty;
            byte cantidad = 0;

            using var comando = conexion.CreateCommand();
            List<(string itemName, float probabilityDrops, byte min, byte max)> dropsProbabilities = new();

            comando.CommandText = @"
                SELECT Drops, ProbabilityToDropItem, MinItemsToDrop, MaxItemsToDrop 
                FROM EnemyDrops 
                WHERE EnemyName = @nombreEnemigo";
            comando.Parameters.AddWithValue("@nombreEnemigo", enemyName);

            using var dataReader = comando.ExecuteReader();
            while (dataReader.Read())
            {
                string itemUniqueName = dataReader.GetString(0);
                float probability = dataReader.GetFloat(1);
                byte minItems = dataReader.GetByte(2);
                byte maxItems = dataReader.GetByte(3);
                dropsProbabilities.Add((itemUniqueName, probability, minItems, maxItems));
            }

            if (dropsProbabilities.Count < 1) return (String.Empty, 0);

            float roulette = MathF.Round(_rng.NextSingle() * 100f, 2);
            float maxProbabilityNumber = MathF.Round(dropsProbabilities[0].probabilityDrops, 2);
            roulette = Math.Clamp(roulette, 0, maxProbabilityNumber);

            for (int i = dropsProbabilities.Count - 1; i >= 0; i--)
            {
                if (roulette <= dropsProbabilities[i].probabilityDrops)
                {
                    nombreItem = dropsProbabilities[i].itemName;
                    cantidad = (byte)_rng.Next(dropsProbabilities[i].min, dropsProbabilities[i].max + 1);
                    dropsProbabilities.Clear();
                    break;
                }
            }

            return (enemyDropName: nombreItem, dropsQuantity: cantidad);
        }

        public static double GettingDropValue(SqliteConnection conexion, string itemName)
        {
            using var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT ValueDrop FROM EnemyDrops WHERE Drops = @nombreItem";
            comando.Parameters.AddWithValue("@nombreItem", itemName);

            using var reader = comando.ExecuteReader();
            if (reader.Read()) return reader.GetDouble(0);

            return 0.00;
        }

        public static void OpenConnection()
        {
            _conectarBase ??= new SqliteConnection(_stringConnection);

            if (_conectarBase.State != System.Data.ConnectionState.Open)
            {
                _conectarBase.Open();
            }
        }
        public static void CloseConnection()
        {
            if (_conectarBase != null && _conectarBase.State == System.Data.ConnectionState.Open)
            {
                _conectarBase.Close();
            }
        }
        public static SqliteConnection GetConnection()
        {
            if (_conectarBase == null || _conectarBase.State != System.Data.ConnectionState.Open)
            {
                OpenConnection();
            }
            return _conectarBase!;
        }

        private static void InsertDrops(SqliteConnection conexion, string nombreEnemigo, string item, float probabilidad, int minItemsSoltar, int maxItemsSoltar, double valor)
        {
            using var checkEnemy = conexion.CreateCommand();
            checkEnemy.CommandText = "SELECT COUNT(*) FROM Enemy WHERE Name = @EnemyName";
            checkEnemy.Parameters.AddWithValue("@EnemyName", nombreEnemigo);

            int count = Convert.ToInt32(checkEnemy.ExecuteScalar());
            if (count > 0)
            {
                using var insertDrop = conexion.CreateCommand();
                insertDrop.CommandText = @"
                    INSERT INTO EnemyDrops (EnemyName, Drops, ProbabilityToDropItem, MinItemsToDrop, MaxItemsToDrop, ValueDrop)
                    VALUES (@EnemyName, @Drops, @Probability, @MinItems, @MaxItems, @ValueDrop);
                ";
                insertDrop.Parameters.AddWithValue("@EnemyName", nombreEnemigo);
                insertDrop.Parameters.AddWithValue("@Drops", item);
                insertDrop.Parameters.AddWithValue("@Probability", probabilidad);
                insertDrop.Parameters.AddWithValue("@MinItems", minItemsSoltar);
                insertDrop.Parameters.AddWithValue("@MaxItems", maxItemsSoltar);
                insertDrop.Parameters.AddWithValue("@ValueDrop", valor);

                insertDrop.ExecuteNonQuery();
            }
        }
    }
}