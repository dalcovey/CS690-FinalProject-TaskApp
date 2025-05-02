using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaskApp.Models;
using Spectre.Console;

namespace TaskApp.Services
{
    public class EventService
    {
        private readonly string _filePath;
        private List<Event> _events = new();

        // Default constructor
        public EventService() : this("events.json") { }

        // Constructor for tests
        public EventService(string filePath)
        {
            _filePath =  filePath;
            LoadEvents();
        }

        public void AddEvent(string name, DateTime date)
        {
            int newId = _events.Any() ? _events.Max(e => e.Id) + 1 : 1;
            _events.Add(new Event { Id = newId, Name = name, Date = date });
            SaveEvents();
        }

        public void RemoveEventByName(string name)
        {
            int removedCount = _events.RemoveAll(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Console.Clear();
            if (removedCount == 0)
            {
                AnsiConsole.MarkupLine("[red]No event found with that name.[/]");
            }
            else
            {
                SaveEvents();
                AnsiConsole.MarkupLine("[green]Event removed.[/]");
            }
        }

        public List<Event> GetEvents()
        {
            return _events;
        }

        public void AssignVolunteerToEvent(int eventId, int volunteerId)
        {
            var ev = _events.FirstOrDefault(e => e.Id == eventId);
            Console.Clear();
            if (ev != null && !ev.VolunteerIds.Contains(volunteerId))
            {
                ev.VolunteerIds.Add(volunteerId);
                SaveEvents();
                AnsiConsole.MarkupLine("[green]Volunteer assigned to event.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Event not found or volunteer already assigned.[/]");
            }
        }

        public void AssignVendorToEvent(int eventId, int vendorId)
        {
            var ev = _events.FirstOrDefault(e => e.Id == eventId);
            Console.Clear();
            if (ev != null && !ev.VendorIds.Contains(vendorId))
            {
                ev.VendorIds.Add(vendorId);
                SaveEvents();
                AnsiConsole.MarkupLine("[green]Vendor assigned to event.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Event not found or vendor already assigned.[/]");
            }
        }

        private void LoadEvents()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _events = JsonSerializer.Deserialize<List<Event>>(json) ?? new();
            }
        }

        private void SaveEvents()
        {
            string json = JsonSerializer.Serialize(_events, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}