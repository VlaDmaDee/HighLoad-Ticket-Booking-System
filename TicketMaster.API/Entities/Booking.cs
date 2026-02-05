namespace TicketMaster.API.Entities {
    public class Booking {
        public int Id { get; set; }
        public int TicketEventId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
