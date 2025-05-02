using System;
using System.IO;
using System.Linq;
using TaskApp.Models;
using TaskApp.Services;
using Xunit;

namespace TaskApp.Tests
{
    public class VolunteerServiceTests : IDisposable
    {
        private readonly string _testFilePath = "test_volunteers.json";
        private VolunteerService _service;

        public VolunteerServiceTests()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            _service = new VolunteerService(_testFilePath);
        }

        [Fact]
        public void AddVolunteer_ShouldAddVolunteerToList()
        {
            _service.AddVolunteer("Alice");

            var result = _service.GetVolunteers();
            Assert.Single(result);
            Assert.Equal("Alice", result.First().Name);
        }

        [Fact]
        public void RemoveVolunteerByName_ShouldRemoveVolunteer()
        {
            _service.AddVolunteer("Bob");
            _service.RemoveVolunteerByName("Bob");

            var result = _service.GetVolunteers();
            Assert.Empty(result);
        }

        [Fact]
        public void RemoveVolunteerByName_NonExistent_ShouldDoNothing()
        {
            _service.AddVolunteer("Charlie");
            _service.RemoveVolunteerByName("NonExistent");

            var result = _service.GetVolunteers();
            Assert.Single(result);
            Assert.Equal("Charlie", result.First().Name);
        }

        [Fact]
        public void Volunteers_ShouldPersistBetweenInstances()
        {
            _service.AddVolunteer("Diana");

            var newService = new VolunteerService(_testFilePath);
            var result = newService.GetVolunteers();

            Assert.Single(result);
            Assert.Equal("Diana", result.First().Name);
        }

        public void Dispose()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }
    }
    public class VendorServiceTests : IDisposable
    {
        private readonly string _testFilePath = "test_vendors.json";
        private VendorService _service;

        public VendorServiceTests()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            _service = new VendorService(_testFilePath);
        }

        [Fact]
        public void AddVendor_ShouldAddVendorToList()
        {
            _service.AddVendor("Acme Supplies");

            var result = _service.GetVendors();
            Assert.Single(result);
            Assert.Equal("Acme Supplies", result.First().Name);
        }

        [Fact]
        public void RemoveVendorByName_ShouldRemoveVendor()
        {
            _service.AddVendor("TechCorp");
            _service.RemoveVendorByName("TechCorp");

            var result = _service.GetVendors();
            Assert.Empty(result);
        }

        [Fact]
        public void RemoveVendorByName_NonExistent_ShouldDoNothing()
        {
            _service.AddVendor("LogiServe");
            _service.RemoveVendorByName("NonExistent");

            var result = _service.GetVendors();
            Assert.Single(result);
            Assert.Equal("LogiServe", result.First().Name);
        }

        [Fact]
        public void Vendors_ShouldPersistBetweenInstances()
        {
            _service.AddVendor("MegaMart");

            var newService = new VendorService(_testFilePath);
            var result = newService.GetVendors();

            Assert.Single(result);
            Assert.Equal("MegaMart", result.First().Name);
        }

        public void Dispose()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }
    }
    public class EventServiceTests : IDisposable
    {
        private readonly string _testFilePath = "test_events.json";
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _eventService = new EventService(_testFilePath);
        }

        [Fact]
        public void AddEvent_ShouldAddEventWithCorrectProperties()
        {
            var date = new DateTime(2025, 5, 1);
            _eventService.AddEvent("Test Event", date);
            var events = _eventService.GetEvents();

            Assert.Single(events);
            Assert.Equal("Test Event", events[0].Name);
            Assert.Equal(date, events[0].Date);
        }

        [Fact]
        public void RemoveEventByName_ShouldRemoveCorrectEvent()
        {
            _eventService.AddEvent("Removable Event", DateTime.Now);
            Assert.Single(_eventService.GetEvents());

            _eventService.RemoveEventByName("Removable Event");

            Assert.Empty(_eventService.GetEvents());
        }

        [Fact]
        public void AssignVolunteerToEvent_ShouldAddVolunteerId()
        {
            _eventService.AddEvent("With Volunteer", DateTime.Now);
            var eventId = _eventService.GetEvents()[0].Id;

            _eventService.AssignVolunteerToEvent(eventId, 99);
            var updatedEvent = _eventService.GetEvents().First();

            Assert.Contains(99, updatedEvent.VolunteerIds);
        }

        [Fact]
        public void AssignVendorToEvent_ShouldAddVendorId()
        {
            _eventService.AddEvent("With Vendor", DateTime.Now);
            var eventId = _eventService.GetEvents()[0].Id;

            _eventService.AssignVendorToEvent(eventId, 42);
            var updatedEvent = _eventService.GetEvents().First();

            Assert.Contains(42, updatedEvent.VendorIds);
        }

        public void Dispose()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}
