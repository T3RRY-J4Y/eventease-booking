using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }
    }
}
