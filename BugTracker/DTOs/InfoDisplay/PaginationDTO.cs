namespace BugTracker.DTOs.InfoDisplay
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int maxAmountPerPage = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > maxAmountPerPage ? maxAmountPerPage : value);
            }
        }
    }
}
