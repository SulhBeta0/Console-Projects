//Finished at 6th April, 2025 -
//Simple visualizer of some famous searching & sorting algorithms.

public class Program
{
    public static void Main()
    {
        int[] numsToSortOrSearch = [2, 45, 718, 1, 0, 63, 2, 34, 99, -1, 7, -37, -281, 14, 62, 77, 9, 1]; //Can change

        Console.Write("Write like this to see some algorithms\n");
        Console.WriteLine("sort/search [typeof] ");
        Console.WriteLine("\n\n[typeof]\nLinear / Binary (Search)\nBubble / Selection / Insertion (Sort)");

        Console.Write("=> ");
        string decision = Console.ReadLine()!.ToLower(), fWord, sWord = String.Empty;

        string[] separator = decision.Split(" ", 2);

        if (separator.Length > 1)
        {
            fWord = separator[0];
            sWord = separator[1];
        }
        else { fWord = separator[0]; }


        Console.Clear();
        switch (fWord)
        {
            case "sort":
                if (sWord.Equals("bubble")) { BubbleSort(numsToSortOrSearch); }

                else if (sWord.Equals("selection")) { SelectionSort(numsToSortOrSearch); }

                else if (sWord.Equals("insertion")) { InsertionSort(numsToSortOrSearch); }

                else { Console.WriteLine("That option is not available."); }
                break;

            case "search":
                if (sWord.Equals("linear")) { LinearSearch(numsToSortOrSearch); }

                else if (sWord.Equals("binary"))
                {
                    Console.WriteLine("This needs to be sorted first to use it.\nWrite what type you would like to use:\n");
                    Console.WriteLine("Bubble | Selection | Insertion\n");

                    string sortMethod = Console.ReadLine()!.ToLower();
                    BinarySearch(numsToSortOrSearch, sortMethod);
                }

                else { Console.WriteLine("That option is not available."); }
                return;

            default:
                Console.WriteLine("That option is not available.");
                return;
        }
        Console.WriteLine("Your list -");
        Printing(numsToSortOrSearch);
    }

    private static void Printing(int[] myArr)
    {
        foreach (int item in myArr)
        {
            Console.Write("| " + item + " |");
        }
        Console.WriteLine("\n");
    }

    private static void LinearSearch(int[] arr)
    {
        Printing(arr);
        bool isSorted = false;
        int movements = 0;
        for (int i = 1; i <= arr.Length; i++)
        {
            if (arr[i] < arr[i - 1])
            {
                isSorted = true;
            }
            else { break; }
        }

        if (isSorted.Equals(false))
        {
            Console.Write("\n\nType a number => ");
            int choice = int.Parse(Console.ReadLine()!);

            int counter = 0;
            foreach (int num in arr)
            {
                if (num.Equals(choice) == false)
                {
                    Console.WriteLine("- Not found!. Index {0}", counter);
                    counter++;
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nNeeded {0} movements to search.\nNumber found! = Index {1}", movements, counter);
                    Console.ResetColor();
                    return;
                }
                movements++;
            }
        }
        else {
            Console.Write("\n\nType a number => ");
            int choice = int.Parse(Console.ReadLine()!);

            int counter = 0;
            foreach (int num in arr)
            {
                if (choice <= num)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nNeeded {0} movements to search.\nNumber found! = Index {1}", movements, counter);
                    Console.ResetColor();
                    return;
                }
                else {
                    Console.WriteLine("+ Not found!. Index {0}", counter);
                    counter++;
                }
                movements++;
            }
        }
    }
    private static void BinarySearch(int[] arr, string sortingDecision)
    {
        switch (sortingDecision)
        {
            case "bubble":
                BubbleSort(arr);
                break;
            case "selection":
                SelectionSort(arr);
                break;
            case "insertion":
                InsertionSort(arr);
                break;
            default:
                Console.WriteLine("That option is not available.");
                return;
        }
        Printing(arr);

        Console.Write("\n\nType a number => ");
        int choice = int.Parse(Console.ReadLine()!);

        int low = 0, high = arr.Length - 1, movements = 0;
        while (low <= high)
        {
            int mid = (low + high) / 2;

            if (choice > arr[mid])
            {
                Console.WriteLine("Number not found!. Index {0}", mid);
                low = mid + 1;
            }
            else if (choice < arr[mid])
            {
                Console.WriteLine("Number not found!. Index {0}", mid);
                high = mid - 1;
            }
            else if (choice.Equals(arr[mid]))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nNeeded {0} movements to search.\nNumber found! = Index {1}",movements, mid);
                Console.ResetColor();
                return;
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Number not in the list!.");
                Console.ResetColor();
                return;
            }
            movements++;
        }
    }
    private static void BubbleSort(int[] arr)
    {
        //Printing(arr);
        int temp, movements = 0;
        for (int i = arr.Length-1; i > 0; i--)
        {
            for (int j = 1; j <= i; j++)
            {
                if (arr[j-1] > arr[j])
                {
                    temp = arr[j];
                    arr[j] = arr[j-1];
                    arr[j-1] = temp;
                    movements++;
                }
                //Printing(arr);
                //Thread.Sleep(400);
                //Console.Clear();
            }
        }
        Console.WriteLine("\nNeeded {0} movements to sort!\n", movements);
    }
    private static void SelectionSort(int[] arr)
    {
        //Printing(arr);
        int movements = 0;
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int bigNumber = 0, temp;
            for (int k = 1; k <= i; k++)
            {
                if (arr[k] <= arr[bigNumber])
                {
                    bigNumber = k;
                }
            }
            temp = arr[i];
            arr[i] = arr[bigNumber];
            arr[bigNumber] = temp;
            movements++;

            //Printing(arr);
            //Thread.Sleep(400);
            //Console.Clear();
        }
        Console.WriteLine("\nNeeded {0} movements to sort!\n", movements);
    }
    private static void InsertionSort(int[] arr)
    {
        //Printing(arr);
        int movements = 0, temp;
        for (int i = 1; i <= arr.Length - 1; i++)
        {
            bool sorted = arr[i - 1] > arr[i] ? false : true;

            if (sorted == false)
            {
                int counter = i;
                while (counter > 0 && arr[counter] < arr[counter - 1])
                {
                    temp = arr[counter];
                    arr[counter] = arr[counter - 1];
                    arr[counter - 1] = temp;
                    movements++;

                    //Printing(arr);
                    //Thread.Sleep(400);
                    //Console.Clear();

                    counter--;
                }
            }
        }
        Console.WriteLine("\nNeeded {0} movements to sort!\n", movements);
    }
}
