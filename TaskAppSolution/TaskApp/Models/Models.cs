using System;
using System.Collections.Generic;

namespace TaskApp.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public List<int> VolunteerIds { get; set; } = new();
        public List<int> VendorIds { get; set; } = new();
    }
}