using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketMaster.API.Data;
using TicketMaster.API.Entities;

namespace TicketMaster.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController:ControllerBase {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context) {
            _context = context;
        }

        [HttpPost("create-event")]
        public IActionResult CreateEvent(string name, int totalTickets) {
            var newEvent = new TicketEvent {
                Name = name,
                Date = DateTime.UtcNow.AddDays(30),
                TotalTickets = totalTickets
            };
            _context.TicketEvents.Add(newEvent);
            _context.SaveChanges();

            return Ok(new { message = $" Koncert {name} created. Tickets: {totalTickets}", eventId = newEvent.Id });
        }

        [HttpPost("book-ticket")]
        public IActionResult BookTicket([FromBody] BookingRequest request) {
            try {
                var ticketEvent = _context.TicketEvents.Find(request.EventId);

                if (ticketEvent == null) {
                    return NotFound(new { message = "Event not found" });
                }

                if (ticketEvent.TotalTickets > 0) {
                    ticketEvent.TotalTickets--;

                    var booking = new Booking {
                        //UserId = userId, 
                        TicketEventId = request.EventId,
                        BookingDate = DateTime.UtcNow
                    };
                    _context.Bookings.Add(booking);
                    _context.SaveChanges();
                    return Ok(new { message = $"Ticket booked for event {ticketEvent.Name}", bookingId = booking.Id });
                }
                return BadRequest(new { message = "No tickets available" });
            } 
            catch(DbUpdateConcurrencyException) {
                return Conflict(new { message = "OOPS - someone made it in time buy ticket, try again" });
            } 
            catch (Exception ex) {
                return StatusCode(500, "another error");
            }
        }
    }

    public class BookingRequest {
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}
