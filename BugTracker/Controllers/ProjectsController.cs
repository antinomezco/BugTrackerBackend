using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Project;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Repositories;
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
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectService projectService, IProjectRepository projectRepository, IMapper mapper)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> GetProjects()
        {
            var projects = await _projectRepository.GetProjectsAsync();

            return _mapper.Map<List<ProjectDTO>>(projects);
        }

        [HttpGet("{id:int}", Name = "GetProject")]
        public async Task<ActionResult<ProjectDTOWithPersonnel>> GetProject(int id)
        {
            var project = await _projectRepository.GetProjectAsync(id);
            
            if (project == null)
                return NotFound();

            return _mapper.Map<ProjectDTOWithPersonnel>(project);
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectCreationDTO projectCreationDTO)
        {
            var project = _mapper.Map<Project>(projectCreationDTO);

            _projectRepository.PostProject(project);

            var projectDTO = _mapper.Map<ProjectDTO>(project);

            return CreatedAtRoute("GetProject", new { id = project.Id }, projectDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProject(int id, ProjectUpdateDTO projectUpdateDTO)
        {

            var projectDB = await _projectService.UpdateProject(id);

            if (projectDB== null)
                return NotFound();

            projectDB = _mapper.Map(projectUpdateDTO, projectDB);

            return NoContent();
        }
    }
}
