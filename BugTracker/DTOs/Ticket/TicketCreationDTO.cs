using BugTracker.Entity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketCreationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubmitterId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
