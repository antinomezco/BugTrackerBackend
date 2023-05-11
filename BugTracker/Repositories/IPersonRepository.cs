using BugTracker.DTOs.Person;
using BugTracker.Entity;

namespace BugTracker.Repositories
{
    public interface IPersonRepository
    {
        Task<Person> CheckPersonExists(int id);
        IQueryable<Person> GetQueryablePeople();
        Task<Person> GetPerson(int id);
        Task<Person> GetPersonWithDetails(int id);
        Task<bool> CheckIfUserWithSameEmailExists(PersonCreationDTO personCreationDTO);
        Task SaveToDB(Person person);
        Task SaveChanges();
        void DeletePerson(int id);
    }
}
