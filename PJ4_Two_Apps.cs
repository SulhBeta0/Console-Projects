//Finished on February 5th, 2025 -
//One to play a game to earn and the other to buy & sell items.

using System.Globalization;

namespace GeneralSystem
{
    class App
    {
        public static void Main()
        {
            UserManagement player = new();


            Console.WriteLine("-Type (ACCESS) to see your options-");
            do {
                Console.Write("\nSystem -> ");
                UserManagement.userGeneralDecision = Console.ReadLine().ToLower();
                
                GeneralInputManager(UserManagement.userGeneralDecision);
            }
            while (UserManagement.userGeneralDecision != "exit" && UserManagement.userGeneralDecision != "");
        }

        public static void GeneralInputManager(string decision)
        {
            //Datos -
            string[] separatingWords = decision.Split(" ", 2);
            string primeraPalabra = string.Empty, segundaPalabra = string.Empty;


            if (separatingWords.Length > 1)
            {
                primeraPalabra = separatingWords[0];
                segundaPalabra = separatingWords[1];
            }
            else { primeraPalabra = separatingWords[0]; }

            switch (primeraPalabra)
            {
                case "see":
                    if (segundaPalabra == "money") { UserManagement.GetMoney(); }

                    else if (segundaPalabra == "inventory") { UserManagement.GetInventory(); }

                    else { Console.WriteLine("See what?"); }
                    break;

                case "shop":
                    Console.Clear();

                    Console.WriteLine("What's your decision?\n+ Buy [item_from_list]\n+ Sell [your_item/s]\n");
                    UserManagement.ShopSystem(UserManagement.userActionDecision);

                    Console.Clear();
                    break;

                case "play":
                    Console.Clear();
                    Console.CursorVisible = false;

                    UserManagement.GameSystem();
                    Console.WriteLine("Bye!");
                    Thread.Sleep(3000);

                    Console.Clear();
                    Console.CursorVisible = true;
                    break;

                case "access":
                    UserManagement.Options();
                    break;

                default:
                    if (decision != "exit" && decision != "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{decision} doesn't exist");
                        Console.ResetColor();
                    }
                    break;
            }
        }
    }

    class UserManagement
    {
        private static readonly Dictionary<string, float> myInv = new();

        public static float Money { get; private set; }

        public static string userGeneralDecision = "";
        public static string userActionDecision = "";

        public static void ShopSystem(string generalDecision)
        {
            while (generalDecision != "return")
            {
                Console.Write("Type (return) to go back -> ");
                generalDecision = Console.ReadLine().ToLower();

                switch (generalDecision)
                {
                    case "buy":
                        Console.Clear();
                        
                        Console.WriteLine("Welcome to our Buy System!\n\nHere's the list with the available objects\n");
                        Thread.Sleep(750);
                        GameObjects.ShowingObjectsList();
                        if (CanBuy())
                        {
                            Console.Write("Which one? => ");
                            string buyOpt = Console.ReadLine().ToLower();

                            GameObjects.BuyableItem(buyOpt);
                        }
                        else {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: You're poor.. Go get some money\n");
                            Console.ResetColor();
                        }
                        break;

                    case "sell":
                        Console.Clear();

                        Console.WriteLine("Welcome to our Sell System!\n");
                        Thread.Sleep(750);
                        if (CanSell())
                        {
                            float valueOpt;

                            GetInventory();

                            Console.Write("\nWhich one? => ");
                            string sellOpt = Console.ReadLine().ToLower();
                            foreach (var item in myInv)
                            {
                                if (item.Key.Equals(sellOpt))
                                {
                                    SellingItem(sellOpt);

                                    Console.Write("For how much? => ");
                                    if (float.TryParse(Console.ReadLine(), out valueOpt)) { AddMoney(valueOpt); }

                                    break;
                                }
                                else {
                                    Console.WriteLine("Error: That item is not in your inventory");
                                    continue;
                                }
                            }
                        }
                        else {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: Buy things!\n");
                            Console.ResetColor();
                        }
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-{0}- doesn't exist.\n", generalDecision);
                        Console.ResetColor();
                        break;
                }
            }
        }

        public static void GameSystem()
        {
            Random rng = new();
            int chances = 5;
            int score = 0;

            Console.WriteLine("Welcome to Play & Earn!");
            Thread.Sleep(500);

            Console.WriteLine("Press (space) and earn money!");
            while (chances > 0)
            {
                int aiGameDecision = rng.Next(1, 11), userGameDecision = rng.Next(1, 11);

                if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n--User[{userGameDecision}] vs AI[{aiGameDecision}]--");
                    Console.ResetColor();

                    if (aiGameDecision > userGameDecision)
                    {
                        Console.WriteLine("AI won this round!");
                        chances--;
                        Console.WriteLine("You have {0} chances\n", chances);
                    }
                    else if (aiGameDecision < userGameDecision)
                    {
                        Console.WriteLine("User won this round!\n");
                        score += 10;
                        AddMoney(60.4f);
                    }
                    else { Console.WriteLine("It's a tie!\n"); }
                }
                else { Console.WriteLine("You didn't pressed the spacebar"); }
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("--You scored -> {0}pts--", score);
            Console.ResetColor();
        }

        public static float AddMoney(float sizeToIncrement)
        {
            Money += sizeToIncrement;
            return Money;
        }

        public static float RemoveMoney(float sizeToDecrement)
        {
            Money -= sizeToDecrement;
            return Money;
        }

        public static void GetMoney()
        {
            CultureInfo language = new("en-US");

            Console.ForegroundColor = ConsoleColor.Green;
            string showMoney = string.Format(language, "--Your account has {0:C}--", Money);
            
            Console.WriteLine(showMoney);
            Console.ResetColor();
        }

        public static void AddIntoInv(string item, float valor)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string puedeMeter = myInv.TryAdd(item, valor) ?
                puedeMeter = $"The {item} has been added to your inventory!\n" :
                puedeMeter = $"The {item} has not been added to your inventory!. You can't have duplicities.\n";

            Console.WriteLine(puedeMeter);
            Console.ResetColor();
        }

        public static void GetInventory()
        {
            CultureInfo language = new("en-US");
            
            Console.WriteLine("- Bought -");
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (CanSell())
            {
                foreach (KeyValuePair<string, float> objeto in myInv)
                {
                    string dinero = string.Format(language, "{0:C}", objeto.Value);
                    Console.WriteLine("+ -{0}- at {1}", objeto.Key, dinero);
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You do not own anything yet");
                Console.ResetColor();
            }
            Console.ResetColor();
        }

        private static void SellingItem(string itemName) => myInv.Remove(itemName);

        public static bool CanBuy() => Money > 0.0f;

        public static bool CanSell() => myInv.Count > 0;

        public static void Options() => Console.WriteLine("\n- Play\n- Shop\n- See [Money / Inventory]\n- Exit...");
    }

    class GameObjects
    {
        private static readonly Dictionary<string, float> items = new()
            {
                {"pincel", 10.00f},
                {"calculadora_rtx", 75.99f},
                {"destornillador", 34.99f},
                {"cinta", 15.50f},
                {"lampara_led", 172.00f},
                {"bateria_iones", 24999.99f},
                {"proyector_8k", 130000.00f},
                {"bocina", 89.99f},
                {"camara", 300.00f},
                {"iphone_pro", 1299.99f},
                {"teclado_jpg", 2000.00f},
                {"refrigerador", 3499.99f},
                {"microfono", 500.00f},
                {"cachimba", 1999.00f},
                {"xxx_peli", 666.66f},
                {"mujer", 69.00f},
                {"esclavo_islandia", 799.99f},
                {"kiwi", 12345.67f},
                {"titanic_sin_estrellar", 999999.99f},
                {"axe_rey_monarca_sombras", 333333.00f},
                {"facturas_shakira", 0.01f}
            };
        private static readonly Dictionary<string, float> updatedItems = new();


        private static Dictionary<string, float> ObjectsListAvailable() //First
        {
            Random rng = new();

            Dictionary<string, float> objectsSelected = new();
            var llavesDict = items.Keys.ToArray();

            for (int i = 0; i < 3; i++)
            {
                string nombreObjeto = llavesDict[rng.Next(llavesDict.Length)];
                float valor = items[nombreObjeto];

                bool metido = objectsSelected.TryAdd(nombreObjeto, valor);
            }
            return objectsSelected;
        }

        private static Dictionary<string, float> KeepingObjects() //Second
        {
            foreach (var item in ObjectsListAvailable())
            {
                bool meter = updatedItems.TryAdd(item.Key, item.Value);
            }
            return updatedItems;
        }

        public static void ShowingObjectsList() //Third
        {
            CultureInfo language = new("en-US");

            foreach (KeyValuePair<string, float> objeto in KeepingObjects())
            {
                try {
                    if (objeto.Key.Count() > 0)
                    {
                        string showingValues = string.Format(language, "{0:C}", objeto.Value);
                        Console.WriteLine($"+ {objeto.Key.ToUpper()} -> " + showingValues);
                    }
                    else {
                        Console.WriteLine("System is under maintenance...\n");
                        break;
                    }
                }
                catch (IndexOutOfRangeException e) { Console.WriteLine("ERROR: {0}", e.Message); }
            }
        }

        public static void BuyableItem(string item) //Fourth
        {
            foreach (KeyValuePair<string, float> objetos in KeepingObjects())
            {
                if (objetos.Key.Equals(item) && UserManagement.Money >= objetos.Value)
                {
                    UserManagement.AddIntoInv(objetos.Key, objetos.Value);
                    UserManagement.RemoveMoney(objetos.Value);
                    break;
                }
                else { continue; }
            }
            updatedItems.Clear();
        }
    }
}
