using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Person> CheckPersonExists(int id)
        {
            return await _context.Personnel.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
