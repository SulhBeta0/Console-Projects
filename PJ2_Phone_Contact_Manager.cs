//Finished on January 27th, 2025 -

using System.Collections.Generic;

public static void Main()
{
    int decision;
    int lenghtContactosOneSesion;
    string numContactos;
    string nombContactos;

    Dictionary<string, string> contacts = new();


    Console.WriteLine("MENU --");
    MoreOptions();
    do
    {
        Console.Write("\n-Menu -> ");
        int.TryParse(Console.ReadLine(), out decision);
        if (decision == 1)
        {
            Console.WriteLine("How many contacts want to insert now?");
            int.TryParse(Console.ReadLine(), out lenghtContactosOneSesion);

            Console.WriteLine("\n- Insert them-");
            for (int i = 0; i < lenghtContactosOneSesion; i++)
            {
                Console.Write("\nContact Name -> ");
                nombContactos = Console.ReadLine().ToLower();
                Console.Write("\nContact Number -> ");
                numContactos = Console.ReadLine();

                contacts.Add(nombContactos, numContactos);
            }
        }
        else if (decision == 2){
            bool hasContacts = contacts.Count > 0;
            string findPerson;

            if (hasContacts) 
            {
                foreach (string contact in contacts.Keys)
                {
                    Console.WriteLine("+ " + contact);
                }
                Console.Write("Which person would you like to see its info?: ");
                findPerson = Console.ReadLine().ToLower();

                if (contacts.Keys.Contains(findPerson))
                {
                    Console.WriteLine("\n-Info-\nName: {0}\nNumber: {1}\n-----------------------------", findPerson, contacts[findPerson]);
                }
            }
            else {
                Console.WriteLine("Nothing to see yet. Add some contacts to start to see them.");
            }
        }
        else if (decision == 3) {
            bool hasContacts = contacts.Count > 0;
            string update;

            if (hasContacts)
            {
                foreach (string contact in contacts.Keys)
                {
                    Console.WriteLine("+ " + contact);
                }
                Console.Write("Which person would you like to update its info?: ");
                update = Console.ReadLine().ToLower();

                if (contacts.Keys.Contains(update))
                {
                    string updateNumber;

                    Console.Write("New number: ");
                    updateNumber = Console.ReadLine();

                    contacts[update] = updateNumber;
                }
                Console.WriteLine(update + " has updated info. Go check it!");
            }
            else
            {
                Console.WriteLine("Nothing to see yet. Add some contacts to use this option.");
            }
        }
        else if (decision == 4) {
            bool hasContacts = contacts.Count > 0;
            string deletePerson;

            if (hasContacts)
            {
                foreach (string contact in contacts.Keys)
                {
                    Console.WriteLine("+ " + contact);
                }
                Console.Write("Which person would you like to delete from your list?: ");
                deletePerson = Console.ReadLine().ToLower();

                if (contacts.Keys.Contains(deletePerson))
                {
                    contacts.Remove(deletePerson);
                }
                Console.WriteLine(deletePerson + " has been deleted from your list!");
            }
            else
            {
                Console.WriteLine("Nothing to see yet. Add some contacts to use this option.");
            }
        }
        else if (decision == 5) {
            bool hasContacts = contacts.Count > 0;

            if (hasContacts)
            {
                Console.WriteLine("-Here's the list of your contacts -");
                foreach (var contact in contacts)
                {
                    Console.WriteLine("Name: {0}\nNumber: {1}\n-------------------", contact.Key, contact.Value);
                }
            }
            else
            {
                Console.WriteLine("Nothing to see yet. Add some contacts to use this option.");
            }
        }
        else if (decision == 6) {
            MoreOptions();
        }
        else if (decision == 7) {
            Console.Clear();
        }
        else if (decision == 8) {
            Console.WriteLine("Bye!");
        }
        else
        {
            Console.WriteLine("That option doens't exit");
            break;
        }
    }
    while (decision != 8);
}

public static void MoreOptions()
{
    Console.WriteLine("\n1.Add contacts\n2.View Contacts\n3.Update Contact\n4.Delete Contact\n5.List Contacts\n6.See Options\n7.Clear Screen\n8.Exit \n");
}
