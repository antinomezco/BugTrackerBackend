using BugTracker.DTOs.Ticket;

namespace BugTracker.Repositories
{
    public interface ITicketRepository
    {
        Task<List<TicketInfoDTO>> GetTicketList(int id);
    }
}
