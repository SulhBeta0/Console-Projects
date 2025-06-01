//Finished on January 31st, 2025 -

public class Pruebas
{
    public static void Main()
    {
        ShoppingCart carrito = new();

        string decision;
        string nameItem;
        float priceItem;
        bool stopProgram;

        Console.WriteLine("Welcome!\nWhat would you like to do? Type -Options-");

        do
        {
            Console.Write("\n+ Menu -> ");
            decision = Console.ReadLine().ToLower();

            stopProgram = (decision != "exit" && decision != "");
            switch (decision)
            {
                case "add":
                    int lenghtPerBuy;

                    Console.WriteLine("- Add Menu - ");
                    Console.WriteLine("How many items are you going to add?");
                    int.TryParse(Console.ReadLine(), out lenghtPerBuy);

                    for (int i=0; i < lenghtPerBuy; i++)
                    {
                        Console.Write("\nName -> ");
                        nameItem = Console.ReadLine().ToLower();

                        Console.Write("Price -> ");
                        float.TryParse(Console.ReadLine(), out priceItem);

                        carrito.AddItem(nameItem, priceItem);
                    }
                    break;

                case "remove":
                    if (carrito.HasItems())
                    {
                        Console.Write("\nName -> ");
                        nameItem = Console.ReadLine().ToLower();

                        carrito.RemoveItem(nameItem);
                        Console.WriteLine("-{0}- has been removed from your cart ^^", nameItem);
                    }
                    else { Console.WriteLine("Add items to use this option."); }
                    break;

                case "list":
                    if (carrito.HasItems())
                    {
                        carrito.ShowItemList();
                    }
                    else { Console.WriteLine("Is empty"); }
                    break;

                case "option":
                    Console.WriteLine("\n- Add\n- Remove\n- List\n- Option");
                    break;

                default:
                    if (stopProgram != false)
                    {
                        Console.WriteLine("{0} doesn't exit.", decision);
                    }
                    break;
            }
        }
        while (stopProgram != false);
    }
}

public class ShoppingCart
{
    private readonly Dictionary<string,float> _items = new();

    public void AddItem(string name, float price) { _items.Add(name, price); }

    public void RemoveItem(string name) { _items.Remove(name); }

    public void ShowItemList() { GetItemList(); }

    public bool HasItems() => _items.Count > 0;

    private void GetItemList()
    {
        CultureInfo language = new("en-US");
        float total= 0.0f;

        foreach (KeyValuePair<string, float> item in _items)
        {
            total += item.Value;
            string precios = string.Format(language, "{0:C2}", item.Value);
            
            Console.WriteLine("\nName: {0}\nPrice: {1}\n--------------------------", item.Key, precios);
        }
        string precioTotal = string.Format(language, "{0:C2}", total);
        Console.WriteLine($"+ Total: {precioTotal}");
    }
}
