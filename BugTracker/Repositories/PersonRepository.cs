using BugTracker.DTOs.Person;
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
        public IQueryable<Person> GetQueryablePeople()
        {
            return _context.Personnel.AsQueryable();
        }
        public async Task<Person> GetPersonWithDetails(int id)
        {
            return await _context.Personnel
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> CheckIfUserWithSameEmailExists(PersonCreationDTO personCreationDTO)
        {
            return await _context.Personnel.AnyAsync(x => x.Email == personCreationDTO.Email);
        }
        public async Task<Person> GetPerson(int id)
        {
            return await _context.Personnel
                .Include(personDB => personDB.PersonnelProjects)
                .ThenInclude(personnelProjectDb => personnelProjectDb.Project)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task SaveToDB(Person person)
        {
            _context.Add(person);
            await SaveChanges();
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public void DeletePerson(int id)
        {
            _context.Remove(new Person() { Id = id});
        }
    }
}
