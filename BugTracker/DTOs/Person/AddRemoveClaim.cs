using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Person
{
    public class AddRemoveClaim
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
