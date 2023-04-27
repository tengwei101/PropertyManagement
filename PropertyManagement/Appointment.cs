using System;
using System.Collections.Generic;

namespace PropertyManagement
{
    public class Attendee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }

    public class Appointment
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string Duration { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public List<Attendee> Attendees { get; set; }
        public string PropertyId { get; set; }
    }
}
