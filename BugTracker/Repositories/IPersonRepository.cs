using BugTracker.Entity;

namespace BugTracker.Repositories
{
    public interface IPersonRepository
    {
        Task<Person> CheckPersonExists(int id);
    }
}
