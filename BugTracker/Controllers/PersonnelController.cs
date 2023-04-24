using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.Entity;
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

        public PersonnelController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonDTO>>> Get()
        {
            var personnel = await _context.Personnel.ToListAsync();

            return Ok(personnel);
        }


        [HttpGet("{id:int}", Name = "GetPerson")]
        public async Task<ActionResult<PersonDTOWithProjects>> Get(int id)
        {
            var person = await _context.Personnel
                .Include(personDB => personDB.PersonnelProjects)
                .ThenInclude(personnelProjectDb => personnelProjectDb.Project)
                .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PersonDTOWithProjects>(person);
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
