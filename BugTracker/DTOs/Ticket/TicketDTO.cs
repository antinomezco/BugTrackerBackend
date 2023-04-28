using BugTracker.Entity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int SubmitterPersonId { get; set; }
        public string SubmitterPersonName { get; set; }
        public int PersonAssignedId { get; set; }
        public PersonProject PersonAssigned { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
