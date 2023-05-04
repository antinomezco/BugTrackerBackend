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

        public async Task<List<TicketInfoDTO>> GetTicketList(int id)
        {
            var tickets = await _context.Tickets
                .Where(ticketDB => ticketDB.AssignedPersonId == id)
                .Select(ticketDB => new TicketInfoDTO
                {
                    Ticket = ticketDB,
                    SubmitterPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.SubmitterPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault(),
                    AssignedPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.AssignedPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault(),
                    ProjectName = _context.Projects
                        .Where(project => project.Id == ticketDB.ProjectId)
                        .Select(project => project.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();
            return tickets;
        }

        public async Task<TicketInfoDTO> GetTicket(int projectId, int id)
        {
            var ticket = await _context.Tickets
                .Where(ticketDB => ticketDB.ProjectId == projectId)
                .Select(ticketDB => new TicketInfoDTO
                {
                    Ticket = ticketDB,
                    SubmitterPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.SubmitterPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault(),
                    AssignedPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.AssignedPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault(),
                    ProjectName = _context.Projects
                        .Where(project => project.Id == ticketDB.ProjectId)
                        .Select(project => project.Name)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync(x => x.Ticket.Id == id);

            return ticket;
        }

        public async Task<Ticket> SaveTicketToDatabase(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
    }
}
