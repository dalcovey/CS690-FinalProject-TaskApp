namespace TaskApp;

using System;
using TaskApp.Models;
using TaskApp.Services;
class Program
{
    static void Main(string[] args)
    {
        var vendorService = new VendorService();
        var volunteerService = new VolunteerService();
        var eventService = new EventService();
        
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
                    VendorsMenu(vendorService);
                    break;
                case "2":
                case "volunteers":
                    VolunteersMenu(volunteerService);
                    break;
                case "3":
                case "events":
                    EventsMenu(eventService, volunteerService, vendorService);
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

    static void VendorsMenu(VendorService vendorService)
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
                    Console.Write("Enter vendor name: ");
                    string name = Console.ReadLine();
                    vendorService.AddVendor(name);
                    Console.WriteLine("Vendor added.");
                    break;

                case "2":
                case "remove":
                    Console.WriteLine("Enter vendor name to remove: ");
                    string removeName = Console.ReadLine();
                    vendorService.RemoveVendorByName(removeName);
                    break;
                    
                case "3":
                case "list":
                    var vendors = vendorService.GetVendors();
                    Console.WriteLine("\n--- Vendors ---");
                    foreach (var v in vendors)
                    {
                        Console.WriteLine($"ID: {v.Id} | Name: {v.Name}");
                    }
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

    static void VolunteersMenu(VolunteerService volunteerService)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.WriteLine("\n--- Volunteer Menu ---");
                Console.WriteLine("1. Add Volunteer");
                Console.WriteLine("2. Remove Volunteer");
                Console.WriteLine("3. List Volunteers");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Select an option (1-4): ");

                string choice = Console.ReadLine().Trim().ToLower();

                switch (choice)
                {
                    case "1":
                    case "add":
                        Console.Write("Enter volunteer name: ");
                        string name = Console.ReadLine();
                        volunteerService.AddVolunteer(name);
                        Console.WriteLine("Volunteer added.");
                        break;

                    case "2":
                    case "remove":
                        Console.Write("Enter volunteer name to remove: ");
                        string removeName = Console.ReadLine();
                        volunteerService.RemoveVolunteerByName(removeName);
                        break;

                    case "3":
                    case "list":
                        var volunteers = volunteerService.GetVolunteers();
                        Console.WriteLine("\n--- Volunteers ---");
                        foreach (var v in volunteers)
                        {
                            Console.WriteLine($"ID: {v.Id} | Name: {v.Name}");
                        }
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
    
    static void EventsMenu(EventService eventService, VolunteerService volunteerService, VendorService vendorService)
    {
        bool backToMain = false;

        while (!backToMain)
        {
            Console.WriteLine("\n--- Event Menu ---");
            Console.WriteLine("1. Add Event");
            Console.WriteLine("2. Remove Event");
            Console.WriteLine("3. List Events");
            Console.WriteLine("4. Assign Volunteer to Event");
            Console.WriteLine("5. Assign Vendor to Event");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Select an option (1-6): ");

            string choice = Console.ReadLine().Trim().ToLower();

        switch (choice)
        {
            case "1":
            case "add":
                Console.Write("Enter event name: ");
                string name = Console.ReadLine();

                Console.Write("Enter event date (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    eventService.AddEvent(name, date);
                    Console.WriteLine("Event added.");
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                }
                break;

            case "2":
            case "remove":
                Console.Write("Enter event name to remove: ");
                string removeName = Console.ReadLine();
                eventService.RemoveEventByName(removeName);
                break;

            case "3":
            case "list":
                var events = eventService.GetEvents();
                var allVolunteers = volunteerService.GetVolunteers();
                var allVendors = vendorService.GetVendors();

                Console.WriteLine("\n--- Events ---");

                foreach (var e in events)
                {
                    Console.WriteLine($"ID: {e.Id} | Name: {e.Name} | Date: {e.Date.ToShortDateString()}");

                    var volunteerNames = e.VolunteerIds
                        .Select(id => allVolunteers.FirstOrDefault(v => v.Id == id)?.Name ?? $"(Unknown ID {id})")
                        .ToList();

                    var vendorNames = e.VendorIds
                        .Select(id => allVendors.FirstOrDefault(v => v.Id == id)?.Name ?? $"(Unknown ID {id})")
                        .ToList();

                    Console.WriteLine($"  Volunteers: {string.Join(", ", volunteerNames)}");
                    Console.WriteLine($"  Vendors: {string.Join(", ", vendorNames)}");
                }
                break;

            case "4":
                Console.Write("Enter event ID: ");
                int eventIdVol = int.Parse(Console.ReadLine());

                Console.Write("Enter volunteer ID to assign: ");
                int volunteerId = int.Parse(Console.ReadLine());

                eventService.AssignVolunteerToEvent(eventIdVol, volunteerId);
                break;

            case "5":
                Console.Write("Enter event ID: ");
                int eventIdVend = int.Parse(Console.ReadLine());

                Console.Write("Enter vendor ID to assign: ");
                int vendorId = int.Parse(Console.ReadLine());

                eventService.AssignVendorToEvent(eventIdVend, vendorId);
                break;

            case "6":
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
