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
                Console.Clear();
                var menu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold blue]Main Menu[/]")
                        .PageSize(10)
                        .AddChoices("Vendors", "Volunteers", "Events", "Exit"));

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
                        .Title("[blue]Vendor Menu[/]")
                        .PageSize(5)
                        .AddChoices("Add Vendor", "Remove Vendor", "List Vendors", "Back to Main Menu"));

                switch (vendorMenu)
                {
                    case "Add Vendor":
                        var name = AnsiConsole.Ask<string>("Enter vendor name:");
                        vendorService.AddVendor(name);
                        Console.Clear();
                        AnsiConsole.MarkupLine("[green]Vendor added.[/]");
                        break;

                    case "Remove Vendor":
                        var removeName = AnsiConsole.Ask<string>("Enter vendor name to remove:");
                        vendorService.RemoveVendorByName(removeName);
                        break;

                    case "List Vendors":
                        var vendors = vendorService.GetVendors();
                        Console.Clear();
                        AnsiConsole.Write(new Rule("[blue]Vendors[/]").RuleStyle("grey").Centered());
                        foreach (var v in vendors)
                        {
                            AnsiConsole.MarkupLine($"[blue]ID:[/] {v.Id} | [blue]Name:[/] {v.Name}");
                        }
                        break;

                    case "Back to Main Menu":
                        Console.Clear();
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
                var volunteerMenu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[blue]Volunteer Menu[/]")
                    .PageSize(5)
                    .AddChoices("Add Volunteer", "Remove Volunteer", "List Volunteers", "Back to Main Menu"));

                switch (volunteerMenu)
                {
                    case "Add Volunteer":
                        var name = AnsiConsole.Ask<string>("Enter volunteer name:");
                        volunteerService.AddVolunteer(name);
                        Console.Clear();
                        AnsiConsole.MarkupLine("[green]Volunteer added.[/]");
                        break;

                    case "Remove Volunteer":
                        var removeName = AnsiConsole.Ask<string>("Enter volunteer name to remove:");
                        volunteerService.RemoveVolunteerByName(removeName);
                        break;

                    case "List Volunteers":
                        var volunteers = volunteerService.GetVolunteers();
                        Console.Clear();
                        AnsiConsole.Write(new Rule("[blue]Volunteers[/]").RuleStyle("grey").Centered());
                        foreach (var v in volunteers)
                        {
                            AnsiConsole.MarkupLine($"[yellow]ID:[/] {v.Id} [yellow]| Name:[/] {v.Name}");
                        }
                        break;

                    case "Back to Main Menu":
                        Console.Clear();
                        backToMain = true;
                        break;
                }
            }
        }

        static void EventsMenu(EventService eventService, VolunteerService volunteerService, VendorService vendorService)
        {
            bool backToMain = false;

            while (!backToMain)
            {
                var eventMenu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[blue]Event Menu[/]")
                    .PageSize(7)
                    .AddChoices("Add Event", "Remove Event", "List Events", "Assign Volunteer to Event", "Assign Vendor to Event", "Back to Main Menu"));

                switch (eventMenu)
                {
                    case "Add Event":
                        Console.Clear();
                        var name = AnsiConsole.Ask<string>("Enter event name:");
                        var dateInput = AnsiConsole.Ask<string>("Enter event date (yyyy-mm-dd):");

                        if (DateTime.TryParse(dateInput, out DateTime date))
                        {
                            eventService.AddEvent(name, date);
                            Console.Clear();
                            AnsiConsole.MarkupLine("[green]Event added.[/]");
                        }
                        else
                        {
                            Console.Clear();
                            AnsiConsole.MarkupLine("[red]Invalid date format.[/]");
                        }
                        break;

                    case "Remove Event":
                        var removeName = AnsiConsole.Ask<string>("Enter event name to remove:");
                        eventService.RemoveEventByName(removeName);
                        break;

                    case "List Events":
                        var events = eventService.GetEvents();
                        var allVolunteers = volunteerService.GetVolunteers();
                        var allVendors = vendorService.GetVendors();

                        Console.Clear();
                        AnsiConsole.Write(new Rule("[blue]Events[/]").RuleStyle("grey").Centered());

                        foreach (var e in events)
                        {
                            AnsiConsole.MarkupLine($"[blue]ID:[/] {e.Id} | [blue]Name:[/] {e.Name} | [blue]Date:[/] {e.Date:yyyy-MM-dd}");

                            var volunteerNames = e.VolunteerIds
                                .Select(id => allVolunteers.FirstOrDefault(v => v.Id == id)?.Name ?? $"(Unknown ID {id})")
                                .ToList();

                            var vendorNames = e.VendorIds
                                .Select(id => allVendors.FirstOrDefault(v => v.Id == id)?.Name ?? $"(Unknown ID {id})")
                                .ToList();

                            AnsiConsole.MarkupLine($"  [yellow]Volunteers:[/] {string.Join(", ", volunteerNames)}");
                            AnsiConsole.MarkupLine($"  [yellow]Vendors:[/] {string.Join(", ", vendorNames)}");
                        }
                        break;

                    case "Assign Volunteer to Event":
                        {
                            var eventChoices = eventService.GetEvents()
                                .Select(e => new SelectionItem<Event>(e, $"{e.Name} ({e.Date:yyyy-MM-dd})"))
                                .ToList();

                            if (!eventChoices.Any())
                            {
                                AnsiConsole.MarkupLine("[red]No events available.[/]");
                                break;
                            }

                            var selectedEvent = AnsiConsole.Prompt(
                                new SelectionPrompt<SelectionItem<Event>>()
                                    .Title("Select an event:")
                                    .UseConverter(i => i.Display)
                                    .AddChoices(eventChoices)).Value;

                            var volunteerChoices = volunteerService.GetVolunteers()
                                .Select(v => new SelectionItem<int>(v.Id, $"{v.Name} (ID: {v.Id})"))
                                .ToList();

                            if (!volunteerChoices.Any())
                            {
                                AnsiConsole.MarkupLine("[red]No volunteers available.[/]");
                                break;
                            }

                            var selectedVolunteerId = AnsiConsole.Prompt(
                                new SelectionPrompt<SelectionItem<int>>()
                                    .Title("Select a volunteer to assign:")
                                    .UseConverter(i => i.Display)
                                    .AddChoices(volunteerChoices)).Value;

                            eventService.AssignVolunteerToEvent(selectedEvent.Id, selectedVolunteerId);
                        }
                        break;

                    case "Assign Vendor to Event":
                        {
                            var eventChoices = eventService.GetEvents()
                                .Select(e => new SelectionItem<Event>(e, $"{e.Name} ({e.Date:yyyy-MM-dd})"))
                                .ToList();

                            if (!eventChoices.Any())
                            {
                                AnsiConsole.MarkupLine("[red]No events available.[/]");
                                break;
                            }

                            var selectedEvent = AnsiConsole.Prompt(
                                new SelectionPrompt<SelectionItem<Event>>()
                                    .Title("Select an event:")
                                    .UseConverter(i => i.Display)
                                    .AddChoices(eventChoices)).Value;

                            var vendorChoices = vendorService.GetVendors()
                                .Select(v => new SelectionItem<int>(v.Id, $"{v.Name} (ID: {v.Id})"))
                                .ToList();

                            if (!vendorChoices.Any())
                            {
                                AnsiConsole.MarkupLine("[red]No vendors available.[/]");
                                break;
                            }

                            var selectedVendorId = AnsiConsole.Prompt(
                                new SelectionPrompt<SelectionItem<int>>()
                                    .Title("Select a vendor to assign:")
                                    .UseConverter(i => i.Display)
                                    .AddChoices(vendorChoices)).Value;

                            eventService.AssignVendorToEvent(selectedEvent.Id, selectedVendorId);
                        }
                        break;

                    case "Back to Main Menu":
                        Console.Clear();
                        backToMain = true;
                        break;
                }
            }
        }
    }
    public class SelectionItem<T>
    {
        public T Value { get; }
        public string Display { get; }

        public SelectionItem(T value, string display)
        {
            Value = value;
            Display = display;
        }

        public override string ToString() => Display;
    }
}