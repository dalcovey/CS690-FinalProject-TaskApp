namespace TaskApp;

class Program
{
    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. Vendors");
            Console.WriteLine("2. Volunteers");
            Console.WriteLine("3. Events");
            Console.WriteLine("4. Exit");
            Console.Write("Please select an option (1-4): ");

            string menu = Console.ReadLine().Trim().ToLower();

            switch (menu)
            {
                case "1":
                case "vendors":
                    VendorsMenu();
                    break;
                case "2":
                case "volunteers":
                    //VolunteersMenu(); not created
                    break;
                case "3":
                case "events":
                    //EventsMenu(); not created
                    break;
                case "4":
                case "exit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid operation, please try again.");
                    break;
            }
        }

        
    }

    static void VendorsMenu()
    {
        bool backToMain = false;

        while (!backToMain)
        {
            Console.WriteLine("\n--- Vendor Menu ---");
            Console.WriteLine("1. Add Vendor");
            Console.WriteLine("2. Remove Vendor");
            Console.WriteLine("3. List Vendors");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("Select an option (1-4): ");

            string vendorMenu = Console.ReadLine().Trim().ToLower();

            switch(vendorMenu)
            {
                case "1":
                case "add":
                    Console.WriteLine("Adding vendor... (feature to be implemented)");
                    break;
                case "2":
                case "remove":
                    Console.WriteLine("Removing vendor... (feature to be implemented)");
                    break;
                case "3":
                case "list":
                    Console.WriteLine("Listing vendors... (feature to be implemented)");
                    break;
                case "4":
                case "exit":
                    backToMain = true;
                    break;
                default:
                    Console.WriteLine("Invalid operation, please try again.");
                    break;
            }

        }
    }
}
