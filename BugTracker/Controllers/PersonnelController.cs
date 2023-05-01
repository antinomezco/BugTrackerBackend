using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    [Route("api/personnel")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public PersonnelController(ApplicationDbContext context, IMapper mapper, IProjectService projectService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonDTO>>> Get()
        {
            var personnel = await _context.Personnel.ToListAsync();

            return _mapper.Map<List<PersonDTO>>(personnel);
        }


        [HttpGet("{id:int}", Name = "GetPerson")]
        public async Task<ActionResult<PersonDTOWithProjects>> Get(int id)
        {
            var person = await _context.Personnel
                .Include(personDB => personDB.PersonnelProjects)
                .ThenInclude(personnelProjectDb => personnelProjectDb.Project)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound();

            var temp = _mapper.Map<PersonDTOWithProjects>(person);

            return _mapper.Map<PersonDTOWithProjects>(person);
        }

        [HttpGet("{id:int}/tickets", Name = "GetPersonTickets")]
        public async Task<ActionResult<PersonDTOWithTickets>> GetPersonTickets(int id)
        {
            var person = await _context.Personnel
                .FirstOrDefaultAsync(x => x.Id == id);
            var ticketData = await _context.Tickets
                .Where(ticketDB => ticketDB.AssignedPersonId == id)
                .Select(ticketDB => new
                {   
                    ticket = ticketDB,
                    submitterPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.SubmitterPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault(),
                    assignedPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.AssignedPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault(),
                    projectName = _context.Projects
                        .Where(project => project.Id == ticketDB.ProjectId)
                        .Select(project => project.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (person == null)
                return NotFound();

            var tickets = ticketData.Select(ticket => {
                var ticketDTO = _mapper.Map<TicketDTO>(ticket.ticket);
                ticketDTO.SubmitterPersonName = ticket.submitterPersonName;
                ticketDTO.AssignedPerson = ticket.assignedPersonName;
                ticketDTO.ProjectName = ticket.projectName;

                return ticketDTO;
            }).ToList();

            var personWithTickets = _mapper.Map<PersonDTOWithTickets>(person);

            personWithTickets.Tickets = tickets;

            return personWithTickets;
        }

        [HttpPost]
        public async Task<ActionResult> Post(PersonCreationDTO personCreationDTO)
        {
            var existePersonaConElMismoNombre = await _context.Personnel.AnyAsync(x => x.Name == personCreationDTO.Name);

            if (existePersonaConElMismoNombre)
            {
                return BadRequest($"Ya existe autor con el nombre {personCreationDTO.Name}");
            }

            var person = _mapper.Map<Person>(personCreationDTO);

            _context.Add(person);
            await _context.SaveChangesAsync();

            var PersonDTO = _mapper.Map<PersonDTO>(person);
            return CreatedAtRoute("GetPerson", new {id  = person.Id}, PersonDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, PersonUpdateDTO personUpdateDTO)
        {
            var personDB = await _context.Personnel
                .Include(x=>x.PersonnelProjects)
                .FirstOrDefaultAsync(x=>x.Id== id);
            if (personDB == null)
                return NotFound();

            personDB = _mapper.Map(personUpdateDTO, personDB);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PersonPatchDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            var personDB = await _context.Personnel.FirstOrDefaultAsync(x=>x.Id== id);

            if(personDB == null)
                return NotFound();

            var personDTO = _mapper.Map<PersonPatchDTO>(personDB);

            patchDocument.ApplyTo(personDTO, ModelState);

            var isValid = TryValidateModel(personDTO);

            if(!isValid)
                return BadRequest(ModelState);

            _mapper.Map(personDTO, personDB);

            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
