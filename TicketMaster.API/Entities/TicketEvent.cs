namespace TicketMaster.API.Entities {
    public class TicketEvent {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int TotalTickets { get; set; }
    }
}
