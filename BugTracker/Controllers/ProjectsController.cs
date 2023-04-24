using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Project;
using BugTracker.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> Get()
        {
            var projects = await _context.Projects.ToListAsync();

            return _mapper.Map<List<ProjectDTO>>(projects);
        }

        [HttpGet("{id:int}", Name = "GetProject")]
        public async Task<ActionResult<ProjectDTOWithPersonnel>> Get(int id)
        {
            var project = await _context.Projects
                .Include(projectDB => projectDB.PersonnelProjects)
                .ThenInclude(personnelProjectDB => personnelProjectDB.Person)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if(project == null)
                return NotFound();

            return _mapper.Map<ProjectDTOWithPersonnel>(project);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProjectCreationDTO projectCreationDTO)
        {

            var personnelIds = await _context.Personnel
                .Where(personnelDB => projectCreationDTO.PersonnelId.Contains(personnelDB.Id))
                .Select(x => x.Id).ToListAsync();

            if (projectCreationDTO.PersonnelId.Count != personnelIds.Count)
                return BadRequest("One of the personnel doesn't exist");

            var project = _mapper.Map<Project>(projectCreationDTO);

            _context.Add(project);

            await _context.SaveChangesAsync();

            var projectDTO = _mapper.Map<ProjectDTO>(project);

            return CreatedAtRoute("GetProject", new { id = project.Id }, projectDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProjectUpdateDTO projectUpdateDTO)
        {
            var projectDB = await _context.Projects
                .Include(x => x.PersonnelProjects)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (projectDB== null)
                return NotFound();

            projectDB = _mapper.Map(projectUpdateDTO, projectDB);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
