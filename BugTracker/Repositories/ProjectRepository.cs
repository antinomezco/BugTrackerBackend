﻿using BugTracker.DTOs.Project;
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

        public async Task<List<Project>> GetProjectsAsync()
        {
            var project = await _context.Projects.ToListAsync();
            return project;
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _context.Projects
                .Include(projectDB => projectDB.PersonnelProjects)
                .ThenInclude(personnelProjectDB => personnelProjectDB.Person)
                .Include(projectDB => projectDB.Tickets.Where(t => t.ProjectId == id))
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

        public async Task<Project> CheckProjectExists(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
