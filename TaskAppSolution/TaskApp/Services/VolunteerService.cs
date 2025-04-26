using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaskApp.Models;

namespace TaskApp.Services
{
    public class VolunteerService
    {
        private readonly string _filePath = "volunteers.json";
        private List<Volunteer> _volunteers = new();

        public VolunteerService()
        {
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
            if (removedCount == 0)
            {
                Console.WriteLine("No volunteer found with that name.");
            }
            else
            {
                SaveVolunteers();
                Console.WriteLine("Volunteer removed.");
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