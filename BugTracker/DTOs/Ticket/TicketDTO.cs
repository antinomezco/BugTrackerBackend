using BugTracker.DTOs.Person;
using BugTracker.Entity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SubmitterPersonId { get; set; }
        public string SubmitterPersonName { get; set; }
        public int? AssignedPersonId { get; set; }
        public string AssignedPerson { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class TicketInfoDTO
    {
        public Entity.Ticket Ticket { get; set; }
        public string SubmitterPersonName { get; set; }
        public string AssignedPersonName { get; set; }
        public string ProjectName { get; set; }
    }
}
