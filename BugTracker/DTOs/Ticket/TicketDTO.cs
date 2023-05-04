using BugTracker.DTOs.Person;
using BugTracker.Entity;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.DTOs.Ticket
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SubmitterPersonId { get; set; }
        public string SubmitterId { get; set; }
        public IdentityUser Submitter { get; set; }
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
