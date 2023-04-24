using BugTracker.DTOs.Person;

namespace BugTracker.DTOs.Project
{
    public class ProjectDTOWithPersonnel : ProjectDTO
    {
        public List<PersonDTO> Personnel { get; set; }
    }
}
