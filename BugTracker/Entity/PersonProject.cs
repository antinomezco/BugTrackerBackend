namespace BugTracker.Entity
{
    public class PersonProject
    {
        public int PersonId { get; set; }
        public int ProjectId { get; set; }
        public Person Person { get; set; }
        public Project Project { get; set; }
        //public List<Ticket> Tickets { get; set; }
        //public DateTime AssignedDate { get; set; }
        //public DateTime? CompletedDate { get; set; }
    }
}
