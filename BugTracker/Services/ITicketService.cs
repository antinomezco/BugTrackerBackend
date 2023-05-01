using BugTracker.DTOs.Ticket;

namespace BugTracker.Services
{
    public interface ITicketService
    {
        Task<List<TicketDTO>> GetListTicketDetails(int id);
    }
}
