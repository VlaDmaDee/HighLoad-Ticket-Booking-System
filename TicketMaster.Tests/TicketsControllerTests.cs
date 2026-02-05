using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketMaster.API.Controllers; 
using TicketMaster.API.Data;
using TicketMaster.API.Entities;

namespace TicketMaster.Tests {
    public class TicketsControllerTests {
        private readonly AppDbContext _context;
        private readonly TicketsController _controller;

        public TicketsControllerTests() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);

            _controller = new TicketsController(_context);
        }

        [Fact]
        public void BookTicket_ReturnsBadRequest_WhenNoTicketsAvailable() {
            // Arrange 
            var eventId = 1;
            _context.TicketEvents.Add(new TicketEvent {
                Id = eventId,
                Name = "Sad Concert",
                TotalTickets = 0, 
                Date = DateTime.Now
            });
            _context.SaveChanges();

            var request = new BookingRequest { EventId = eventId, UserId = 100 };

            // Act 
            var result = _controller.BookTicket(request);

            // Assert 
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void BookTicket_ReturnsOk_WhenTicketsExist() {
            // Arrange
            var eventId = 2;
            _context.TicketEvents.Add(new TicketEvent {
                Id = eventId,
                Name = "Happy Concert",
                TotalTickets = 10,
                Date = DateTime.Now
            });
            _context.SaveChanges();

            var request = new BookingRequest { EventId = eventId, UserId = 100 };

            // Act
            var result = _controller.BookTicket(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); 

            var ticketInDb = _context.TicketEvents.Find(eventId);
            ticketInDb.TotalTickets.Should().Be(9); 
        }
    }
}