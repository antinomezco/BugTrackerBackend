using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Project;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BugTracker.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectsController(ApplicationDbContext context, IProjectService projectService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();

            return _mapper.Map<List<ProjectDTO>>(projects);
        }

        [HttpGet("{id:int}", Name = "GetProject")]
        public ActionResult<ProjectDTOWithPersonnel> GetProject(int id)
        {

            var projectWithTicketDetails = _projectService.GetProjectWithTickets(id);

            if (projectWithTicketDetails == null)
                return NotFound();

            return Ok(projectWithTicketDetails);
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectCreationDTO projectCreationDTO)
        {
            var project = _mapper.Map<Project>(projectCreationDTO);

            _projectService.AddProject(project);

            var projectDTO = _mapper.Map<ProjectDTO>(project);

            return CreatedAtRoute("GetProject", new { id = project.Id }, projectDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProject(int id, ProjectUpdateDTO projectUpdateDTO)
        {
            //var projectDB = await _context.Projects
            //    .Include(x => x.PersonnelProjects)
            //    .FirstOrDefaultAsync(x => x.Id == id);
            var projectDB = _projectService.UpdateProject(id);

            if (projectDB== null)
                return NotFound();

            projectDB = _mapper.Map(projectUpdateDTO, projectDB);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
