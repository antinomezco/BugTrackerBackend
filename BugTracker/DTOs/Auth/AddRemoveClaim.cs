using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Auth
{
    public class AddRemoveClaim
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
