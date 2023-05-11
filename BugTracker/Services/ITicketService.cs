using BugTracker.DTOs.Ticket;
using BugTracker.Entity;

namespace BugTracker.Services
{
    public interface ITicketService
    {
        Task<List<TicketDTO>> GetListTicketDetails(int id);
        IQueryable<TicketDTO> GetQueryableTicketDetails(int id);
        Task<TicketDTOWithDetails> GetTicketDetails(int projectId, int id);
        Task<List<TicketDTO>> GetTicketsDetails(int id);
        Task<Project> IsProjectExist(int projectId);
        Task<Person> IsSubmitterExist(int submitterPersonId);
        Task<Ticket> SaveTicket(Ticket ticket);
    }
}
