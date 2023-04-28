using System.ComponentModel.DataAnnotations;

namespace BugTracker.Entity
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public int SubmitterPersonId { get; set; }
        public List<TicketPerson> AssignedPeople { get; set; }

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
