using BugTracker.Entity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Person
{
    public class PersonCreationDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [EnumDataType(typeof(PersonRole))]
        public PersonRole Role { get; set; } = PersonRole.User;
    }
}
