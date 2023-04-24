using BugTracker.DTOs.Project;

namespace BugTracker.DTOs.Person
{
    public class PersonDTOWithProjects : PersonDTO
    {
        public List<ProjectDTO> Projects { get; set; }
    }
}
