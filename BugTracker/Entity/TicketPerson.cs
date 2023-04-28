namespace BugTracker.Entity
{
    public class TicketPerson
    {
        public int TicketId { get; set; }
        public int PersonId { get; set; }
        public Ticket Ticket { get; set; }
        public Person Person { get; set; }
    }
}
