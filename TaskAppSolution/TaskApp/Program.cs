using System;
using TaskApp.Models;
using TaskApp.Services;
using Spectre.Console;

namespace TaskApp
{
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
                var menu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold blue]Main Menu[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Vendors", "Volunteers", "Events", "Exit"
                        }));

                switch (menu.ToLower())
                {
                    case "vendors":
                        VendorsMenu(vendorService);
                        break;
                    case "volunteers":
                        VolunteersMenu(volunteerService);
                        break;
                    case "events":
                        EventsMenu(eventService, volunteerService, vendorService);
                        break;
                    case "exit":
                        exit = true;
                        break;
                }
            }
        }

        static void VendorsMenu(VendorService vendorService)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                var vendorMenu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]--- Vendor Menu ---[/]")
                        .PageSize(5)
                        .AddChoices(new[]
                        {
                            "Add Vendor", "Remove Vendor", "List Vendors", "Back to Main Menu"
                        }));

                switch (vendorMenu)
                {
                    case "Add Vendor":
                        var name = AnsiConsole.Ask<string>("Enter vendor name:");
                        vendorService.AddVendor(name);
                        AnsiConsole.MarkupLine("[green]Vendor added.[/]");
                        break;

                    case "Remove Vendor":
                        var removeName = AnsiConsole.Ask<string>("Enter vendor name to remove:");
                        vendorService.RemoveVendorByName(removeName);
                        break;

                    case "List Vendors":
                        var vendors = vendorService.GetVendors();
                        AnsiConsole.WriteLine("\n--- Vendors ---");
                        foreach (var v in vendors)
                        {
                            AnsiConsole.MarkupLine($"[blue]ID:[/] {v.Id} | [blue]Name:[/] {v.Name}");
                        }
                        break;

                    case "Back to Main Menu":
                        backToMain = true;
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
}
