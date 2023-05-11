using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Entity
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int SubmitterId { get; set; }
        public Person SubmitterPerson { get; set; }
        public ApplicationUser Submitter { get; set; }
        public int? AssignedPersonId { get; set; }
        public Person AssignedPerson { get; set; }
        public bool IsResolved { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [EnumDataType(typeof(TicketPriority))]
        public TicketPriority TicketPriority { get; set; }
        [EnumDataType(typeof(TicketStatus))]
        public TicketStatus TicketStatus { get; set; }
        [EnumDataType(typeof(TicketType))]
        public TicketType TicketType { get; set; }
    }
}
