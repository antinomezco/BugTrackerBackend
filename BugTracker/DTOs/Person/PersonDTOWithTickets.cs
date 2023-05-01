using BugTracker.DTOs.Project;
using BugTracker.DTOs.Ticket;

namespace BugTracker.DTOs.Person
{
    public class PersonDTOWithTickets : PersonDTO
    {
        public List<TicketDTO> Tickets { get; set; }
    }
}
