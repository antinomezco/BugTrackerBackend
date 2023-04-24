namespace BugTracker.Entity
{
    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Email{ get; set; }

        public PersonRole Role { get; set; }

        public DateTime CreatedDate  { get; set; }

        public List<PersonProject> PersonnelProjects { get; set; }
    }
}
