using BugTracker.Entity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int PersonAssignedId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public TicketType TicketType { get; set; }
    }
}
