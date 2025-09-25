using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models
{
    public class Event : IValidatableObject
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        [StringLength(150, ErrorMessage = "Event name cannot exceed 150 characters.")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event Date is required.")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        [Required(ErrorMessage = "An event image is required.")]
        public string ImageUrl { get; set; } = string.Empty;

        public Event()
        {
            Bookings = new HashSet<Booking>();
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "End time must be later than start time.",
                    new[] { nameof(EndTime) });
            }
        }
    }
}
