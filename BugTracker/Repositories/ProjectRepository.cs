using BugTracker.Entity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _context.Projects
                .Include(projectDB => projectDB.Tickets)
                .Include(projectDB => projectDB.PersonnelProjects)
                .ThenInclude(personnelProjectDB => personnelProjectDB.Person)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async void PostProject(Project project)
        {
            _context.Add(project);

            await _context.SaveChangesAsync();
        }
        public async Task<Project> PutProject(int id)
        {
            return await _context.Projects
                .Include(x => x.PersonnelProjects)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
