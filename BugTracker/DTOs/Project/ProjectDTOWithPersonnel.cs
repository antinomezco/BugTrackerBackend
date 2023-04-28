using BugTracker.DTOs.Person;
using BugTracker.DTOs.Ticket;

namespace BugTracker.DTOs.Project
{
    public class ProjectDTOWithPersonnel : ProjectDTO
    {
        public List<PersonDTO> Personnel { get; set; }
        public List<TicketDTO> Tickets { get; set; }
    }
}
