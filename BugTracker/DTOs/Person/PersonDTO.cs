using BugTracker.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BugTracker.DTOs.Person
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        [EnumDataType(typeof(PersonRole))]
        public PersonRole Role { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
