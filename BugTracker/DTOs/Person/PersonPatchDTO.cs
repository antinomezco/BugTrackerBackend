using BugTracker.Entity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Person
{
    public class PersonPatchDTO
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }
        [EnumDataType(typeof(PersonRole))]
        public string? Role { get; set; }
    }
}
