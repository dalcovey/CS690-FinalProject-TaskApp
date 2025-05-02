using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaskApp.Models;
using Spectre.Console;

namespace TaskApp.Services
{
    public class VendorService
    {
        private readonly string _filePath;
        private List<Vendor> _vendors = new();

        // Default constructor
        public VendorService() : this("vendors.json") { }

        // Testable constructor
        public VendorService(string filePath)
        {
            _filePath = filePath;
            LoadVendors();
        }

        public void AddVendor(string name)
        {
            int newId = _vendors.Any() ? _vendors.Max(v => v.Id) + 1 : 1;
            _vendors.Add(new Vendor { Id = newId, Name = name });
            SaveVendors();
        }

        public void RemoveVendorByName(string name)
        {
            int removedCount = _vendors.RemoveAll(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Console.Clear();
            if (removedCount == 0)
            {
                AnsiConsole.MarkupLine("[red]No vendor found with that name.[/]");
            }
            else
            {
                SaveVendors();
                AnsiConsole.MarkupLine("[green]Vendor removed.[/]");
            }
        }

        public List<Vendor> GetVendors()
        {
            return _vendors;
        }

        private void LoadVendors()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _vendors = JsonSerializer.Deserialize<List<Vendor>>(json) ?? new();
            }
        }

        private void SaveVendors()
        {
            string json = JsonSerializer.Serialize(_vendors, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}