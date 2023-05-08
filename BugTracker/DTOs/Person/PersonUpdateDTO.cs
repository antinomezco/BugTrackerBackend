using BugTracker.Entity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.DTOs.Person
{
    public class PersonUpdateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Role { get; set; }
    }
}
