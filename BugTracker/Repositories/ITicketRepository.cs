using BugTracker.DTOs.Ticket;
using BugTracker.Entity;

namespace BugTracker.Repositories
{
    public interface ITicketRepository
    {
        Task<List<TicketInfoDTO>> GetTicketList(int id);

        Task<TicketInfoDTO> GetTicket(int projectId, int id);
        Task<Ticket> SaveTicketToDatabase(Ticket ticket);
    }
}
