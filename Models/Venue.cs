using System.Collections.Generic;
using EventEase.Web.Models;
using System.ComponentModel.DataAnnotations;

public class Venue
{
    public int VenueId { get; set; }

    [Required]
    [StringLength(100)]
    public string VenueName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero.")]
    public int Capacity { get; set; }

    [Required]
    [Url]
    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; }

    public Venue()
    {
        Bookings = new HashSet<Booking>();
    }
}
