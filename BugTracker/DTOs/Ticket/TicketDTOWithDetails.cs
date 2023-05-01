using BugTracker.Entity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketDTOWithDetails : TicketDTO
    {
        public string TicketPriority { get; set; }
        public string TicketStatus { get; set; }
        public string TicketType { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
