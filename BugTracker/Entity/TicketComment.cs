namespace BugTracker.Entity
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Person Person { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
