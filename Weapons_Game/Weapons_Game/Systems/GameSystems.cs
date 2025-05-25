using System.Diagnostics;
using System.Globalization;
using Weapons_Game.database;

namespace Weapons_Game.Systems
{
    class Game_Systems
    {
        private readonly PlayerData _data;
        private readonly Player _player;
        private readonly Weapons _weapons;
        private readonly Enemy _enemy;
        private static readonly CultureInfo _lang = new("en-US");

        public Game_Systems(Player jugador, PlayerData datosJugador, Weapons armas, Enemy enemigo)
        {
            _player = jugador;
            _data = datosJugador;
            _weapons = armas;
            _enemy = enemigo;
        }

        public void ProfileSystem()
        {
            string money = String.Format(_lang, "{0:C2}", _data.Money);

            Tools.MessageCreator("--- PROFILE\n");
            Tools.MessageCreator
                (
                    "| NAME => {0}\n| BALANCE => {1} [Last Sell: ↑${2} ]\n| CURRENT WEAPON => {3} [{4}dmg]\n| KILLS => {5}\n",
                    ConsoleColor.DarkCyan,
                    _data.Name, money, _data.LastSell, _data.Weapon.ToString(),
                    _weapons.SharpStats[_data.Weapon.ToString()].damage.ToString(), _data.KillCount
                );

            Tools.MessageCreator("--- WEAPONS");
            foreach (string item in _data.InventoryWeapons)
            {
                Tools.MessageCreator("- {0}", ConsoleColor.Green, item);
            }
            Console.WriteLine();

            if (_data.InventoryResources.Count > 0)
            {
                Tools.MessageCreator("--- INVENTORY");
                foreach (var item in _data.InventoryResources)
                {
                    Tools.MessageCreator("- {0}: x{1}", ConsoleColor.DarkYellow, item.Key, item.Value);
                }
            }
            else { Console.WriteLine(Tools.WarningsCreator("YOU NEED TO HAVE RESOURCES!")); }
        }

        public void ResourcesSystem()
        {
            Stopwatch tempo = new();
            var selectedEnemy = Enemy.NewEnemy();
            double cooldown = _weapons.SetCooldown() * 1000;

            Console.CursorVisible = false;
            _enemy.SetHealth(selectedEnemy.health);

            while (true)
            {
                Tools.MessageCreator("An enemy has appeared!\n{0} [{1}]\n", ConsoleColor.Blue, selectedEnemy.name, _enemy.Health);
                Tools.MessageCreator("\nType: -ESC- to exit | -SPACEBAR- to kill\n");
                Tools.ClearKeyBuffer();

                ConsoleKey llave = Console.ReadKey(true).Key;
                if (llave == ConsoleKey.Spacebar)
                {
                    int newHealth = _player.Attack(_enemy.Health, _weapons.SetDamage());
                    _enemy.SetHealth(newHealth);

                    tempo.Start();
                    Console.WriteLine($"Wait {cooldown / 1000}s!!");
                    while (tempo.IsRunning && tempo.ElapsedMilliseconds < cooldown)
                    {
                        if (cooldown <= 999)
                        {
                            Thread.Sleep((int)cooldown);
                        }
                        else { Thread.Sleep(1000); }
                    }

                    tempo.Stop();
                    tempo.Reset();
                }
                else if (llave == ConsoleKey.Escape) break;
                Console.Clear();

                if (_enemy.Health <= 0)
                {
                    var drops = Enemy.DropSystem(selectedEnemy.name);
                    _data.AddingToInv(drops.Name, drops.Quantity);

                    Tools.MessageCreator("{0} dead!", ConsoleColor.White, selectedEnemy.name);
                    Tools.MessageCreator("Dropped: x{0} {1}", ConsoleColor.Yellow, drops.Quantity, drops.Name);
                    Thread.Sleep(1500);

                    _data.ApproveNewState();
                    Console.Clear();

                    selectedEnemy = Enemy.NewEnemy();
                    _enemy.SetHealth(selectedEnemy.health);
                }
            }
            Console.CursorVisible = true;
            MyDatabase.CloseConnection();
        }

        public void ShopSystem()
        {
            Tools.MessageCreator("Want to 1=(buy) or 2=(sell)?", ConsoleColor.White);
            Console.Write("=> ");
            int choice = int.Parse(Console.ReadLine()!.ToLower());

            Console.Clear();
            switch (choice)
            {
                case 1:
                    Buy();
                    return;

                case 2:
                    Sell();
                    return;

                default:
                    return;
            }
        }

        private void Buy()
        {
            List<string> weaponsList = _weapons.SharpStats.Keys.ToList();

            if (weaponsList.All(weapon => _data.InventoryWeapons.Contains(weapon)))
            {
                Console.WriteLine("\n" + Tools.WarningsCreator("You cannot buy anymore!"));
                return;
            }

            Tools.MessageCreator(" - WEAPON MASTER -\n-----------------------");
            foreach (string weapon in weaponsList)
            {
                CultureInfo lang = new("en-US");
                string money = String.Format(lang, "{0:C2}", _weapons.SharpStats[weapon].price);

                if (_data.InventoryWeapons.Contains(weapon))
                {
                    Tools.MessageCreator("| {0} [ALREADY HAVE]", ConsoleColor.Green, weapon);
                }
                else { Tools.MessageCreator("| {0} {1} [{2}dmg] [{3}s]", ConsoleColor.Green, weapon, money, _weapons.SharpStats[weapon].damage, _weapons.SharpStats[weapon].cooldown); }
            }

            Console.Write("\n\n=> ");
            string decision = Console.ReadLine()!.ToUpper();

            if (!String.IsNullOrEmpty(decision) && _weapons.SharpStats.ContainsKey(decision) && !_data.InventoryWeapons.Contains(decision) && _data.Money >= _weapons.SharpStats[decision].price)
            {
                _data.SubstractingMoney(_weapons.SharpStats[decision].price);
                _data.AddingToInvWeapon(decision);
                Tools.MessageCreator("The -{0}- has been added to your inventory!", ConsoleColor.Yellow, decision);
            }
            else { Console.WriteLine(Tools.WarningsCreator("Something went wrong")); }
        }

        private void Sell()
        {
            string decision = String.Empty;
            while (true)
            {
                if (_data.InventoryResources.Count < 1)
                {
                    Console.WriteLine(Tools.WarningsCreator("NO RESOURCES, NO PLACE FOR YOU HERE!"));
                    MyDatabase.CloseConnection();
                    return;
                }

                Tools.MessageCreator(" - SELLING PLAZA-\n-----------------------");
                foreach (var item in _data.InventoryResources)
                {
                    Tools.MessageCreator("- {0}: x{1} => ${2}/ud", ConsoleColor.DarkYellow, item.Key, item.Value, Enemy.SellValuesDrops(item.Key));
                }
                
                Console.Write("\n\nWhich item you want to sell? Type -exit- from here pls => ");
                decision = Console.ReadLine()!.ToLower();

                if (decision.Contains("exit"))
                { 
                    MyDatabase.CloseConnection();
                    return;
                }

                Console.WriteLine("How many of that?");
                if (!String.IsNullOrEmpty(decision) && _data.InventoryResources.ContainsKey(decision) && uint.TryParse(Console.ReadLine(), out uint decisionQuantity))
                {
                    decisionQuantity = Math.Clamp(decisionQuantity, 0, _data.InventoryResources[decision]);
                    double money = decisionQuantity * Enemy.SellValuesDrops(decision);

                    _data.SetLastSell(money);
                    _data.RemovingFromInv(decision, decisionQuantity);
                    _data.AddingMoney(money);
                    Tools.MessageCreator("You have made a sell!", ConsoleColor.Yellow);
                }
                else { Console.WriteLine(Tools.WarningsCreator("\nSomething went wrong")); }
                
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
