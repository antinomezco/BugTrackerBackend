using AutoMapper;
using BugTracker.DTOs.Project;
using BugTracker.Entity;
using BugTracker.Repositories;
using BugTracker.Services;

namespace BugTracker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITicketService _ticketService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(ITicketService ticketService, IProjectRepository projectRepository, IMapper mapper)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ProjectDTOWithPersonnel> GetProjectWithTickets(int id)
        {
            var project = await _projectRepository.GetProjectAsync(id);

            if(project == null)
                return null;

            var tickets = await _ticketService.GetListTicketDetails(id);

            var projectWithTicketDetails = _mapper.Map<ProjectDTOWithPersonnel>(project);
            projectWithTicketDetails.Tickets = tickets;

            return projectWithTicketDetails;
        }
        public void AddProject(Project project)
        {
            _projectRepository.PostProject(project);
        }

        public Task<Project> UpdateProject(int id)
        {
            return _projectRepository.PutProject(id);
        }
    }
}
