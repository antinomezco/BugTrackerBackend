﻿using BugTracker.Entity;

namespace BugTracker.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetProjectAsync(int id);
        void PostProject(Project project);
        Task<Project> PutProject(int id);
        Task<Project> CheckProjectExists(int id);
        Task<List<Project>> GetProjectsAsync();
    }
}
