using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaskApp.Models;
using Spectre.Console;

namespace TaskApp.Services
{
    public class VolunteerService
    {
        private readonly string _filePath;
        private List<Volunteer> _volunteers = new();

        // Default constructor used by main app
        public VolunteerService() : this("volunteers.json") { }

        //Testable consturctor used by unit test
        public VolunteerService(string filePath)
        {
            _filePath = filePath;
            LoadVolunteers();
        }

        public void AddVolunteer(string name)
        {
            int newId = _volunteers.Any() ? _volunteers.Max(v => v.Id) + 1 : 1;
            _volunteers.Add(new Volunteer { Id = newId, Name = name });
            SaveVolunteers();
        }

        public void RemoveVolunteerByName(string name)
        {
            int removedCount = _volunteers.RemoveAll(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Console.Clear();
            if (removedCount == 0)
            {
                AnsiConsole.MarkupLine("[red]No volunteer found with that name.[/]");
            }
            else
            {
                SaveVolunteers();
                AnsiConsole.MarkupLine("[green]Volunteer removed.[/]");
            }
        }

        public List<Volunteer> GetVolunteers()
        {
            return _volunteers;
        }

        private void LoadVolunteers()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _volunteers = JsonSerializer.Deserialize<List<Volunteer>>(json) ?? new();
            }
        }

        private void SaveVolunteers()
        {
            string json = JsonSerializer.Serialize(_volunteers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}