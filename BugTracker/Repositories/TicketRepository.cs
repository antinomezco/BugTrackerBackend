using AutoMapper;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Ticket>> GetTicketList(int id)
        {
            var tickets = await _context.Tickets
                .Where(ticketDB => ticketDB.ProjectId == id)
                .ToListAsync();
            return tickets;
        }

        public async Task<Ticket> GetTicket(int projectId, int id)
        {
            var ticket = await _context.Tickets
                .Where(ticketDB => ticketDB.ProjectId == projectId)
                .FirstOrDefaultAsync(x => x.Id == id);

            return ticket;
        }

        public async Task<List<Ticket>> GetTickets(int id)
        {
            var tickets = await _context.Tickets
                .Where(ticketDB => ticketDB.AssignedPersonId == id)
                .ToListAsync();

            return tickets;
        }

        public IQueryable<Ticket> GetTicketQueryableList(int id)
        {
            var tickets = _context.Tickets
                .Where(ticketDB => ticketDB.ProjectId == id)
                .AsQueryable();

            return tickets;
        }

        public async Task<Ticket> SaveTicketToDatabase(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
    }
}
