using BugTracker.Entity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketDTOWithDetails : TicketDTO
    {
        public TicketPriority TicketPriority { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
