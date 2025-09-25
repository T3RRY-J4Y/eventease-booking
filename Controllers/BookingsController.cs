using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Web.Data;
using EventEase.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Web.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bookings (with search + details view)
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var query = _context.BookingDetailsView.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b =>
                    b.BookingId.ToString().Contains(searchTerm) ||
                    b.EventName.Contains(searchTerm));
            }

            var results = await query.ToListAsync();
            return View(results);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,VenueId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var selectedEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == booking.EventId);
                if (selectedEvent != null)
                {
                    booking.BookingDate = selectedEvent.EventDate;
                    booking.StartTime = selectedEvent.StartTime;
                    booking.EndTime = selectedEvent.EndTime;
                }

                // Double booking prevention
                bool overlapExists = await _context.Bookings.AnyAsync(b =>
                    b.VenueId == booking.VenueId &&
                    b.BookingDate == booking.BookingDate &&
                    ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                     (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime)));

                if (overlapExists)
                {
                    ModelState.AddModelError("", "This venue is already booked during the selected time.");
                    PopulateDropdowns(booking.EventId, booking.VenueId);
                    return View(booking);
                }

                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while creating the booking. Please try again.");
                }
            }

            PopulateDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            PopulateDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var selectedEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == booking.EventId);
                    if (selectedEvent != null)
                    {
                        booking.BookingDate = selectedEvent.EventDate;
                        booking.StartTime = selectedEvent.StartTime;
                        booking.EndTime = selectedEvent.EndTime;
                    }

                    // Double booking prevention (exclude current booking)
                    bool overlapExists = await _context.Bookings.AnyAsync(b =>
                        b.BookingId != booking.BookingId &&
                        b.VenueId == booking.VenueId &&
                        b.BookingDate == booking.BookingDate &&
                        ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                         (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime)));

                    if (overlapExists)
                    {
                        ModelState.AddModelError("", "This venue is already booked during the selected time.");
                        PopulateDropdowns(booking.EventId, booking.VenueId);
                        return View(booking);
                    }

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while updating the booking. Please try again.");
                }
            }

            PopulateDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            // Prevent deletion if event/venue is still active
            if (booking.Event != null && booking.Event.EventDate >= DateTime.Today)
            {
                TempData["ErrorMessage"] = "You cannot delete a booking for an upcoming event.";
                return RedirectToAction(nameof(Index));
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private void PopulateDropdowns(int? selectedEventId = null, int? selectedVenueId = null)
        {
            var events = _context.Events.ToList();
            var venues = _context.Venues.ToList();

            ViewBag.EventId = new SelectList(events, "EventId", "EventName", selectedEventId)
                                  .Prepend(new SelectListItem { Text = "Select Event", Value = "" });

            ViewBag.VenueId = new SelectList(venues, "VenueId", "VenueName", selectedVenueId)
                                  .Prepend(new SelectListItem { Text = "Select Venue", Value = "" });
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
