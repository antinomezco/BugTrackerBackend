using System.ComponentModel.DataAnnotations;

namespace BugTracker.Entity
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email{ get; set; }
        public PersonRole Role { get; set; }
        public DateTime CreatedDate  { get; set; }
        public List<PersonProject> PersonnelProjects { get; set; }
        public List<TicketPerson> TicketsPeople { get; set; }
    }
    public enum PersonRole
    {
        User,
        Developer,
        Project_manager,
        Admin
    }
}
