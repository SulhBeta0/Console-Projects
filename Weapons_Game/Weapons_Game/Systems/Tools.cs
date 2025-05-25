
namespace Weapons_Game.Systems
{
    static class Tools
    {
        
        public static void MessageCreator(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void MessageCreator(string text, ConsoleColor color = ConsoleColor.Gray, params Object[] data)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(String.Format(text, data));
            Console.ResetColor();
        }

        public static Exception WarningsCreator(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return new Exception(text);
        }

        public static void ClearKeyBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
