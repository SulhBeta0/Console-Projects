//Finished on March 3rd, 2025 -
//If you have a word like this => "super dino", you'll have to enter the literal spacebar to continue guessing. Also happens with the symbols.

public static void Main()
{
    Random rng = new();
    string[] words = {
    "apple", "banana", "cherry", "dragonfruit", "elephant", "forest", "giraffe", "harmony", "island", "jungle",
    "kangaroo", "lemon", "mountain", "notebook", "ocean", "penguin", "quasar", "rainbow", "sunshine", "tiger",
    "umbrella", "volcano", "waterfall", "xylophone", "yogurt", "zebra", "adventure", "butterfly", "cactus", "diamond",
    "eclipse", "firefly", "galaxy", "horizon", "illusion", "jigsaw", "koala", "lantern", "melody", "nebula",
    "orchestra", "puzzle", "quicksand", "radiance", "serenity", "twilight", "universe", "voyage", "whisper", "zenith"
    };

    int lifes = 6;
    int wordChoseIndex = rng.Next(0, words.Length);
    
    var myWord = new string('*', words[wordChoseIndex].Length).ToCharArray();


    do
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        foreach (char character in myWord) {
            Console.Write(character);
        }
        Console.ResetColor();
        
        Console.WriteLine("\nYou have {0} life/s!", lifes);
        Console.WriteLine("\n");

        Console.Write("=> ");
        string myCharacter = Console.ReadKey().KeyChar.ToString();
        Console.WriteLine();

        bool containMyCharacter = words[wordChoseIndex].Contains(myCharacter);
        if (!containMyCharacter) { lifes--; }

        int counter = 0;
        while (containMyCharacter && counter < words[wordChoseIndex].Length)
        {
            bool coincideLetra = words[wordChoseIndex][counter] == (Convert.ToChar(myCharacter));
            
            if (coincideLetra) {
                myWord[counter] = Convert.ToChar(myCharacter);
            }
            counter++;
        }
        Console.Clear();
    }
    while (lifes > 0 && myWord.Contains<char>('*'));
    
    if(myWord.Contains<char>('*'))
    {
        Console.WriteLine("YOU LOSE :(");
        Console.WriteLine("This was the word: {0}", words[wordChoseIndex]);
    }
    else {
        Console.WriteLine("YOU DID IT :D!!");
        Console.WriteLine("This was the word: {0}", words[wordChoseIndex]);
    }
}