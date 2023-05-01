using BugTracker.DTOs.Person;

namespace BugTracker.DTOs.Project
{
    public class ProjectCreationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
