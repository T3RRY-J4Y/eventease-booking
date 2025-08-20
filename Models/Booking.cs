using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "The Event field is required.")]
        [Display(Name = "Event")]
        public int? EventId { get; set; }  // ✅ Nullable
        public Event? Event { get; set; }

        [Required(ErrorMessage = "The Venue field is required.")]
        [Display(Name = "Venue")]
        public int? VenueId { get; set; }  // ✅ Nullable
        public Venue? Venue { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime? BookingDate { get; set; }
    }
}
