using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models
{
    public class Booking : IValidatableObject
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "The Event field is required.")]
        [Display(Name = "Event")]
        public int? EventId { get; set; }
        public Event? Event { get; set; }

        [Required(ErrorMessage = "The Venue field is required.")]
        [Display(Name = "Venue")]
        public int? VenueId { get; set; }
        public Venue? Venue { get; set; }

        [Required(ErrorMessage = "Booking Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime? BookingDate { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public TimeSpan? StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        public TimeSpan? EndTime { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime.HasValue && EndTime.HasValue && EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "End time must be later than start time.",
                    new[] { nameof(EndTime) });
            }

            if (!BookingDate.HasValue)
            {
                yield return new ValidationResult(
                    "A booking date is required.",
                    new[] { nameof(BookingDate) });
            }
        }
    }
}
