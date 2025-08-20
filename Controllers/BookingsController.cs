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

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue);

            return View(await bookings.ToListAsync());
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
                    booking.BookingDate = selectedEvent.EventDate;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            PopulateDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }

        private void PopulateDropdowns(int? selectedEventId = null, int? selectedVenueId = null)
        {
            var events = _context.Events.ToList();
            var venues = _context.Venues.ToList();

            ViewBag.EventId = new SelectList(events, "EventId", "EventName", selectedEventId)
                                  .Prepend(new SelectListItem { Text = "-- Select Event --", Value = "" });

            ViewBag.VenueId = new SelectList(venues, "VenueId", "VenueName", selectedVenueId)
                                  .Prepend(new SelectListItem { Text = "-- Select Venue --", Value = "" });
        }

        // GET: Bookings/Edit/5
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
                        booking.BookingDate = selectedEvent.EventDate;

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
            }

            // Repopulate dropdowns if validation fails
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
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
