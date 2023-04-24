namespace BugTracker.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PersonProject> PersonnelProjects { get; set; }

        public List<Ticket> Tickets { get; set; }

    }
}
