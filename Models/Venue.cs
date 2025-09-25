using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models
{
    public class Venue : IValidatableObject
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters.")]
        public string VenueName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string Location { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero.")]
        public int Capacity { get; set; }

        // Azure Blob Storage
        [Required(ErrorMessage = "Venue image is required.")]
        [Url(ErrorMessage = "Please enter a valid URL for the venue image.")]
        public string ImageUrl { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; }

        public Venue()
        {
            Bookings = new HashSet<Booking>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Capacity <= 0)
            {
                yield return new ValidationResult(
                    "Venue capacity must be greater than zero.",
                    new[] { nameof(Capacity) });
            }
        }
    }
}
