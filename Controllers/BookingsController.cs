using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EventEase.Web.Data;
using EventEase.Web.Models;

namespace EventEase.Web.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(AppDbContext context, ILogger<BookingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .ToListAsync();
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            return booking == null ? NotFound() : View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,VenueId")] Booking booking)
        {
            // Fetch the selected Event
            var selectedEvent = await _context.Events
                .Include(e => e.Venue) // optional if you want to validate venue
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("EventId", "Selected event does not exist.");
            }
            else
            {
                // Set the VenueId automatically from the Event
                booking.VenueId = selectedEvent.VenueId;

                // Set the BookingDate to the EventDate
                booking.BookingDate = selectedEvent.EventDate;
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns if something went wrong
            LoadDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }



        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            LoadDropdowns(booking.EventId, booking.VenueId);
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
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            return booking == null ? NotFound() : View(booking);
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

        private void LoadDropdowns(int? selectedEventId = null, int? selectedVenueId = null)
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", selectedEventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", selectedVenueId);
        }
    }
}
