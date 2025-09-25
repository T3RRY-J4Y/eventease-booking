using System;

namespace EventEase.Web.Models
{
    public class BookingDetailsView
    {
        public int BookingId { get; set; }
        public DateTime? BookingDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventImage { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string VenueImage { get; set; } = string.Empty;
    }
}
