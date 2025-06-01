//Finished on April 25th, 2025 -

using System.Text.Json;

namespace Aprender
{
    public class Pruebas
    {
        public static void Main()
        {
            string decision;

            Console.WriteLine("- Type options -");
            do
            {
                Console.Write("Task Manager -> ");
                decision = Console.ReadLine().ToLower();

                InputProcessor(decision);
            }
            while ((decision != "exit") && (decision != ""));
        }

        public static void InputProcessor(string input)
        {
            string[] wordSeparator = new string[2];
            
            string firstWord = string.Empty,
                   secondWord = string.Empty;


            wordSeparator = input.Split(' ', 2);
            if (wordSeparator.Length > 1)
            {
                firstWord = wordSeparator[0].ToLower();
                secondWord = wordSeparator[1].ToLower();
            }
            else { firstWord = wordSeparator[0].ToLower(); }

            switch (firstWord)
            {
                case "add":
                    if (secondWord == "tag")
                    {
                        Console.Write("Adding Tag -> ");
                        string addingTag = Console.ReadLine();

                        Tareas.AgregarTag(addingTag);
                    }
                    else if (secondWord == "task") {
                        Console.Write("Adding Task -> ");
                        string addingTask = Console.ReadLine();

                        Tareas.AgregarTareas(addingTask);
                    }
                    else { Console.WriteLine("Add what?\n"); }
                    break;

                case "remove":
                    if (secondWord == "task")
                    {
                        if (Tareas.CanAccessTasks())
                        {
                            Console.Write("Removing Task by position-> ");
                            int removingTask;

                            if (int.TryParse(Console.ReadLine(), out removingTask))
                            {
                                Tareas.RemoverTareas(removingTask);
                            }
                        }
                    }
                    else if (secondWord == "tag")
                    {
                        if (Tareas.CanAccessTags())
                        {
                            Console.Write("Removing Tag by name-> ");
                            string removingTag = Console.ReadLine();

                            Tareas.RemoverTag(removingTag);
                        }
                    }
                    else { Console.WriteLine("Remove what?\n"); }
                    break;

                case "see":
                    if (secondWord == "tag")
                    {
                        Tareas.VerTags();
                    }
                    else if (secondWord == "task") {
                        Tareas.VerTarea();
                    }
                    else { Console.WriteLine("See what?\n"); }
                    break;

                case "search":
                    if (Tareas.CanAccessTasks())
                    {
                        Console.Write("Search a Task by its position-> ");
                        int searchingTask;

                        if (int.TryParse(Console.ReadLine(), out searchingTask))
                        {
                            Tareas.SearchTaskData(searchingTask);
                        }
                    }
                    break;

                case "mark":
                    if (Tareas.CanAccessTasks())
                    {
                        int searchingTask2, changingStateOfTask;

                        Console.Write("Search the Task by its position-> ");
                        searchingTask2 = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Remember that, you COULD NOT change it back to its original state. Only by removing the task itself.");
                        
                        Console.Write("Now, change the state of it:\n[0= LEAVE IT AS FALSE]\n[1= TRUE]\n-> ");
                        if (int.TryParse(Console.ReadLine(), out changingStateOfTask))
                        {
                            Tareas.MarkAsDoneTasks(searchingTask2, changingStateOfTask);
                        }
                    }
                    break;

                case "save":
                    Console.Write("Write a name file to save your data: ");
                    string file_name = Console.ReadLine();

                    Tareas.SaveData(file_name);
                    break;

                case "load":
                    Console.Write("Write the name file to load your data: ");
                    string fileName = Console.ReadLine();

                    Tareas.LoadData(fileName);
                    break;

                case "options":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("----------------------\n1.Add [Task/Tag]\n2.Remove [Task/Tag]\n" +
                        "3.See [Task/s | Tag/s]\n4.Search [Single Task Data]\n5.Mark [Task/s as Done]n6.Save Data\n7.Load Data\n------------------------\n");
                    Console.ResetColor();
                    break;

                default:
                    if (firstWord != "exit" && firstWord != "") {
                        Console.WriteLine($"{input} doesn't exist\n"); 
                    }
                    break;
            }
        }
    }

    public class DataStructure
    {
        public Dictionary<string, bool> TaskData { get; set; } = new();
        public List<string> Tags { get; set; } = new();
    }

    public class Tareas
    {
        private static readonly Dictionary<string, bool> _taskData = new();
        private static readonly List<string> _tags = new();

        private static readonly int _maxLenghtOfTasks = 100;
        private static readonly int _maxLenghtOfTags = 5;


        public static bool CanAccessTasks() => _taskData.Count > 0;
        public static bool CanAccessTags() => _tags.Count > 0;

        public static void AgregarTareas(string add, bool verify= false)
        {
            if (_taskData.Count < _maxLenghtOfTasks)
            {
                if (add == "") { Console.WriteLine("You can't leave it blank\n"); }
                else {
                    _taskData.Add(add, verify);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("A task has been entered in your system!\n");
                    Console.ResetColor();
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Your capacity is full (Max |100). Remove some tasks.\n");
                Console.ResetColor();
            }
        }

        public static void AgregarTag(string tag)
        {
            if (_tags.Count < _maxLenghtOfTags)
            {
                if (tag.Contains(' ') || (tag == ""))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR:\n1.You can't add tags with multiple words -\n+ Tag: [Trabajo hecho -> Not permitted] +Tag: [Hecho -> Permitted]\n2. You can't leave it blank\n");
                    Console.ResetColor();
                }
                else {
                    _tags.Add(tag);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("A tag has been added to your system!\n");
                    Console.ResetColor();
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Your capacity is full (Max |5). Remove some tags.\n");
                Console.ResetColor();
            }
        }

        public static void RemoverTareas(int remove)
        {
            if (CanAccessTasks())
            {
                KeyValuePair<string, bool> taskToRemove = _taskData.ElementAt(remove);
                _taskData.Remove(taskToRemove.Key);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The task has been removed from your system!\n");
                Console.ResetColor();
            }
            else { Console.WriteLine("There's nothing to remove yet.\n"); }
        }

        public static void RemoverTag(string remover)
        {
            if (CanAccessTags())
            {
                _tags.Remove(remover);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The tag has been removed from your system!\n");
                Console.ResetColor();
            }
            else { Console.WriteLine("There's nothing to remove yet\n"); }
        }

        public static void VerTarea()
        {
            if (CanAccessTasks()) 
            {
                int counter=0;

                Console.ForegroundColor = ConsoleColor.White;
                foreach (KeyValuePair<string, bool> task in _taskData)
                {
                    Console.WriteLine($"{counter}. Task Description: {task.Key}\nIs it done?: {task.Value}\n");
                    counter++;
                }
                Console.ResetColor();
            }
            else { Console.WriteLine("There's nothing to see yet.\n"); }
        }

        public static void VerTags()
        {
            if (CanAccessTags())
            {
                int counter=0;

                Console.ForegroundColor = ConsoleColor.White;
                foreach (string tag in _tags)
                {
                    Console.WriteLine($"{counter}. [{tag}]\n");
                    counter++;
                }
                Console.ResetColor();
            }
            else { Console.WriteLine("There's nothing to see yet.\n"); }
        }

        public static void SearchTaskData(int index)
        {
            if (CanAccessTasks())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                
                KeyValuePair<string, bool> taskToSearch = _taskData.ElementAt(index);
                Console.WriteLine($"Task Description: {taskToSearch.Key}\nIs it done?: {taskToSearch.Value}\n");

                Console.ResetColor();
            }
            else { Console.WriteLine("There's nothing to search yet.\n"); }
        }

        public static void MarkAsDoneTasks(int indexToFind, int state)
        {
            if (CanAccessTasks())
            {
                KeyValuePair<string, bool> taskToSearch = _taskData.ElementAt(indexToFind);

                try {
                    if ((state == 0) && (_taskData[taskToSearch.Key] != true))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The state of your task has remained as FALSE!\n");
                        Console.ResetColor();
                    }
                    else if (state == 1) {
                        _taskData[taskToSearch.Key] = true;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The state of your task has changed to TRUE!\n");
                        Console.ResetColor();
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR:\n1.That number exceeds the boundaries of the states!\nOr\n2.You tried to change it to FALSE\n");
                        Console.ResetColor();
                    }
                }
                catch (ArgumentOutOfRangeException messg)
                {
                    Console.WriteLine(messg.Message);
                }

            }
            else { Console.WriteLine("There's nothing to mark yet.\n"); }
        }

        public static void SaveData(string fileName= "")
        {
            var dataToSave = new
            {
                TaskData = _taskData,
                Tags = _tags };

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{fileName}.json");
            
            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    string jsonFile = JsonSerializer.Serialize(dataToSave, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(filePath, jsonFile);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Your file has succesfully been saved ^^\n");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: Your file has not been saved. {ex.Message}\n");
                    Console.ResetColor();
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: That name is not availble\n");
                Console.ResetColor();
            }
        }

        public static void LoadData(string fileName= "")
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{fileName}.json");
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonFile = File.ReadAllText(filePath);
                    var data = JsonSerializer.Deserialize<DataStructure>(jsonFile); 

                    if (data != null)
                    {
                        _taskData.Clear();
                        foreach (var item in data.TaskData)
                        {
                            _taskData[item.Key] = item.Value;
                        }

                        _tags.Clear();
                        _tags.AddRange(data.Tags);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Data loaded perfectly ^^\n");
                        Console.ResetColor();
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Warning: The file could be empty or malformed\n");
                        Console.ResetColor();
                    }
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: The file doesn't exit in that path: {filePath}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: I couldn't load you file\n{ex.Message}");
                Console.ResetColor();
            }
        }
    }
}