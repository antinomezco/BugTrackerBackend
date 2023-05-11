using BugTracker.DTOs.Ticket;
using BugTracker.Entity;

namespace BugTracker.Repositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetTicketList(int id);
        Task<Ticket> GetTicket(int projectId, int id);
        Task<List<Ticket>> GetTickets(int id);
        IQueryable<Ticket> GetTicketQueryableList(int id);
        Task<Ticket> SaveTicketToDatabase(Ticket ticket);
    }
}
