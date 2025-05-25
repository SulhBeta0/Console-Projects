using Weapons_Game.Systems;

namespace Weapons_Game
{
    class PlayerData
    {
        public static bool messageAlreadyShowed = false;
        public string Name { get; private set; }
        public double Money { get; private set; }
        public double LastSell { get; private set; }
        public uint KillCount { get; private set; }
        public Weapons.Sharp Weapon { get; private set; }
        public static Enemy.Estados LevelState { get; private set; }
        public Dictionary<string, uint> InventoryResources { get; private set; }
        public List<string> InventoryWeapons { get; private set; }

        public PlayerData(string name)
        {
            Name = String.IsNullOrEmpty(name) ? "Default_00000" : name;
            Money = 300.0;
            LastSell = 0;
            KillCount = 0;
            Weapon = Weapons.Sharp.GAUNTLET;
            LevelState = Enemy.Estados.LV1;
            InventoryResources = new();
            
            InventoryWeapons = new();
            InventoryWeapons.Add(Weapon.ToString());
        }
        public PlayerData() : this("Default_00000") {}

        public void AddingKills() => KillCount += 1;

        public void ChangingWeapons(string weapon) => Weapon = Enum.Parse<Weapons.Sharp>(weapon);

        public void SubstractingMoney(double? value= null)
        {
            double amount = value ?? 0.0;
            Money -= amount;
        }
        
        public void SetLastSell(double? value= null)
        {
            double amount = value ?? 0.0;
            LastSell = amount;
        }

        public void ApproveNewState()
        {
            if (!(KillCount <= 450) && !messageAlreadyShowed)
            {
                LevelState = Enemy.Estados.LV2;
                Tools.MessageCreator("NEW LEVEL ARRIVED!! MORE ENEMIES WILL F*** YOU", ConsoleColor.Cyan);
                messageAlreadyShowed = true;

                Thread.Sleep(3000);
            }
        }

        public void AddingMoney(double? value= null)
        {
            double amount = value ?? 0.0;
            Money += amount;
        }

        public void AddingToInvWeapon(string weaponName) => InventoryWeapons.Add(weaponName);

        public void AddingToInv(string itemName, byte? itemQuantity = null)
        {
            byte amount = itemQuantity ?? 0;
            if(!InventoryResources.TryAdd(itemName, amount))
            {
                InventoryResources[itemName] += amount;
            }
        }
        
        public void RemovingFromInv(string itemName, uint? quantity = null)
        {
            uint amount = quantity ?? 0;
            amount = Math.Clamp(amount, 0, InventoryResources[itemName]);
            if (amount < InventoryResources[itemName])
            {
                InventoryResources[itemName] -= amount;
            }
            else { InventoryResources.Remove(itemName); }
        }

        public void SettingValueFromLoad(string filePath)
        {
            using FileStream fileOption = new(filePath, FileMode.Open);
            using BinaryReader readData = new(fileOption);
            
            Name = readData.ReadString();
            Money = readData.ReadDouble();
            KillCount = readData.ReadUInt32();
            messageAlreadyShowed = readData.ReadBoolean();
            Weapon = (Weapons.Sharp)readData.ReadInt32();
            LevelState = (Enemy.Estados)readData.ReadInt32();

            InventoryWeapons.Clear();
            int weaponCount = readData.ReadInt32();
            for (int i = 0; i < weaponCount; i++)
            {
                string weapon = readData.ReadString();
                InventoryWeapons.Add(weapon);
            }

            InventoryResources.Clear();
            int resourceCount = readData.ReadInt32();
            for (int i = 0; i < resourceCount; i++)
            {
                string key = readData.ReadString();
                uint value = readData.ReadUInt32();
                InventoryResources[key] = value;
            }
        }
    }

    class Player
    {
        private readonly PlayerData _data;
        private static readonly Random _rng = new();

        public Player(PlayerData data) => _data = data;

        public void Write(int input, Game_Systems game)
        {
            Console.Clear();
            switch (input)
            {
                case 1:
                    game.ResourcesSystem();
                    break;
                case 2:
                    game.ShopSystem();
                    break;
                case 3:
                    game.ProfileSystem();
                    break;
                case 4:
                    ChangingWeapon();
                    break;
                case 5:
                    Save();
                    break;
                case 6:
                    Load();
                    break;
                case 7:
                    return;
                default:
                    Tools.MessageCreator("That option do not exist!", ConsoleColor.Red);
                    break;
            }
            Console.ResetColor();
            Console.Write("\nPress a key to exit");
            Console.ReadKey(true);

            Console.Clear();
        }

        public int Attack(int enemyHealth, int weaponDamage)
        {
            int newHealth;
            if( _rng.NextSingle() <= Weapons.probabilityBetterDamage)
            {
                newHealth = enemyHealth -= weaponDamage*2;
                Tools.MessageCreator("2X Damage", ConsoleColor.White);
                Thread.Sleep(35);
            }
            else { newHealth = enemyHealth -= weaponDamage; }

            if (enemyHealth <= 0)  _data.AddingKills();

            return newHealth;
        }

        private void Save()
        {
            SavingStructure saving = new()
            {
                MyName = _data.Name,
                MyMoney = _data.Money,
                MyKillCount = _data.KillCount,
                MyWeapon = _data.Weapon,
                MyMessageForNewState = PlayerData.messageAlreadyShowed,
                MyLevelState = PlayerData.LevelState,
                MyInventoryResources = _data.InventoryResources,
                MyInventoryWeapons = _data.InventoryWeapons
            };

            string myFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "my_game_data.bin");
            try
            {
                FileStream fileOption = new(myFilePath, FileMode.Create);
                BinaryWriter writeData = new(fileOption);

                writeData.Write(saving.MyName);
                writeData.Write(saving.MyMoney);
                writeData.Write(saving.MyKillCount);
                writeData.Write(saving.MyMessageForNewState);
                writeData.Write((int)saving.MyWeapon);
                writeData.Write((int)saving.MyLevelState);

                writeData.Write(saving.MyInventoryWeapons.Count);
                foreach (string item in saving.MyInventoryWeapons)
                {
                    writeData.Write(item);
                }

                writeData.Write(saving.MyInventoryResources.Count);
                foreach (var item in saving.MyInventoryResources)
                {
                    writeData.Write(item.Key);
                    writeData.Write(item.Value);
                }

                writeData.Dispose();
                fileOption.Dispose();

                Tools.MessageCreator("Your file has been succesfully saved", ConsoleColor.Yellow);
            }
            catch (Exception) { Console.WriteLine(Tools.WarningsCreator("ERROR: YOUR FILE HAS NOT BEEN SAVED")); }
        }
        private void Load()
        {
            string myFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "my_game_data.bin");

            try {
                if (File.Exists(myFilePath))
                {
                    _data.SettingValueFromLoad(myFilePath);
                    Tools.MessageCreator("Your file has been succesfully loaded", ConsoleColor.Yellow);
                }
                else { Console.WriteLine(Tools.WarningsCreator("ERROR: YOUR FILE DO NOT EXIST")); }
            }
            catch (Exception) { Console.WriteLine(Tools.WarningsCreator("AN ERROR HAS APPEARD. STOPPED LOADING PROCESS")); }
        }
        private void ChangingWeapon()
        {
            if (_data.InventoryWeapons.Count < 2)
            {
                Console.WriteLine(Tools.WarningsCreator("You don't have more weapons. Go buy some!"));
                return;
            }

            Tools.MessageCreator("Option/s to change your current weapon\n");
            foreach (string weapon in _data.InventoryWeapons)
            {
                if(weapon != _data.Weapon.ToString())
                {
                    Tools.MessageCreator("| {0}", ConsoleColor.Green, weapon);
                }
            }

            Console.Write("\n=> ");
            string decision = Console.ReadLine()!.ToUpper();
            if(!String.IsNullOrEmpty(decision) && _data.InventoryWeapons.Contains(decision))
            {
                _data.ChangingWeapons(decision);
                Tools.MessageCreator("You're carrying a new weapon!", ConsoleColor.Yellow);
            }
        }
    }
}
