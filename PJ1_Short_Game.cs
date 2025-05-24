// Finished on January 15th, 2025 -

using Aprender;

class Game
{
    static void Main(string[] args)
    {
        User jugador = new();
        Enemy ordenador = new();

        string decisiones = string.Empty;
        
        Console.WriteLine("- THE GAME STARTS! -");
        User.Options();


        while ((jugador.vida > 0 && ordenador.health > 0) && (decisiones != "surrender"))
        {
            int userAttacking = RollingDice();
            int aiAttacking = RollingDice();

            Console.WriteLine("=============================");
            decisiones = Console.ReadLine().ToLower();
            Console.WriteLine("=============================");

            if (decisiones == "")
            {
                break;
            }
            else {User.OutputUserMessage(decisiones);}

            switch (decisiones) {
                case "attack":
                    Console.WriteLine("\nUser[{0}]\n=======\nPC[{1}]\n", userAttacking, aiAttacking);

                    if (userAttacking > aiAttacking)
                    {
                        Console.WriteLine("User dealt more damage");
                        int amount = Damaged(userAttacking, ref ordenador.health);

                        Console.WriteLine("\nPC health || " + amount + "\n");
                    }
                    else if (userAttacking < aiAttacking)
                    {
                        Console.WriteLine("PC dealt more damage");
                        int amount_2 = Damaged(aiAttacking, ref jugador.vida);

                        Console.WriteLine("\nMy health || " + amount_2 + "\n");
                    }
                    else { Console.WriteLine("It's a tie. Nothing happened."); }
                    break;

                case "menu":
                    User.Options();
                    break;

                case "see":
                    jugador.GetHealth();
                    break;
                
                default:
                    if (decisiones == "surrender")
                    {
                        Console.WriteLine("OK...");
                    }
                    else { Console.WriteLine("That command doesn't exist /..."); }
                    break;
            }
        }
        Console.WriteLine("\n=============================");
        Console.WriteLine("PC health || " + ordenador.health);
        Console.WriteLine("My health || " + jugador.vida);

        if ((jugador.vida <= 0) || (jugador.vida < ordenador.health))
        {
            Console.WriteLine("\nPC is the Winner!!!");
        }
        else if ((ordenador.health <= 0) || (ordenador.health < jugador.vida)) {
            Console.WriteLine("\nUser is the Winner!!!");
        }
        else {Console.WriteLine("\nIt's a tie...");}
    }

    private static int RollingDice()
    {
        Random rng = new();

        int dice = rng.Next(1, 31);
        return dice;
    }

    private static int Damaged(int daño, ref int life) {
        life -= daño;
        return life;
    }
} 
// ENEMY:
namespace Aprender {
    public class Enemy
    {
        public int health = 150;
    }
}

// USER:
namespace Aprender {
    public class User
    {
        public int vida = 150;

        public static void Options() {
            Console.Write("\nYou have this available actions -\n1.Attack.\n2.Menu\n3.See [health].\n4.Surrender...\n");
        }

        public static void OutputUserMessage(string option) {
            Console.WriteLine("\nUser has used the -{0}- option.", option);
        }

        public void GetHealth() {
            Console.WriteLine("\nMy health || " + vida + "\n");
        }
    }
}
