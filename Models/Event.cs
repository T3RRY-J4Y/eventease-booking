using System;
using System.Collections.Generic;
using EventEase.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event Date is required.")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }  // Nullable navigation property

        public ICollection<Booking> Bookings { get; set; }

        public Event()
        {
            Bookings = new HashSet<Booking>();
        }
    }

}
