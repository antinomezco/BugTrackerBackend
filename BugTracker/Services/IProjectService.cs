using BugTracker.DTOs.Project;
using BugTracker.Entity;

namespace BugTracker.Services
{
    public interface IProjectService
    {
        Task<ProjectDTOWithPersonnel> GetProjectWithTickets(int id);
        Task<Project> UpdateProject(int id);
    }
}
