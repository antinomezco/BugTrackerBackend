using BugTracker.DTOs.Person;

namespace BugTracker.DTOs.Project
{
    public class ProjectUpdateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<int> PersonnelId { get; set; }
    }
}
