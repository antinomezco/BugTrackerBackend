using BugTracker.Entity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Person
{
    public class PersonCreationDTO
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        [Required]
        [EnumDataType(typeof(PersonRole))]
        public string? Role { get; set; }
    }
}
