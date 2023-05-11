using AutoMapper;
using BugTracker.DTOs.InfoDisplay;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Repositories;
using BugTracker.Services;
using BugTracker.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    [Route("api/personnel")]
    [ApiController]
        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public PersonnelController(IPersonRepository personRepository, ITicketService ticketService,IMapper mapper)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult<List<PersonDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _personRepository.GetQueryablePeople();
            await HttpContext.InsertPaginationParametersInHeader(queryable);
            var personnel = await queryable.OrderBy(person=>person.Name).Paginate(paginationDTO).ToListAsync();

            return _mapper.Map<List<PersonDTO>>(personnel);
        }

        [HttpGet("{id:int}", Name = "GetPerson")]
        [AllowAnonymous]
        public async Task<ActionResult<PersonDTOWithProjects>> Get(int id)
        {
            var person = await _personRepository.GetPerson(id);

            if (person == null)
                return NotFound();

            return _mapper.Map<PersonDTOWithProjects>(person);
        }

        //[HttpGet("{id:int}/tickets", Name = "GetPersonTickets")]
        //[AllowAnonymous]
        //public async Task<ActionResult<PersonDTOWithTickets>> GetPersonTickets(int id)
        //{
        //    var person = await _personRepository.GetPersonWithDetails(id);
        //    var ticketData = await _ticketService.GetTicketsDetails(id);

        //    if (person == null)
        //        return NotFound();

        //    var tickets = ticketData.Select(ticket => {
        //        var ticketDTO = _mapper.Map<TicketDTO>(ticket);
        //        ticketDTO.SubmitterPersonName = ticket.SubmitterPersonName;
        //        ticketDTO.AssignedPerson = ticket.AssignedPerson;
        //        ticketDTO.Description = ticket.Description;
        //        ticketDTO.ProjectId = ticket.ProjectId;
        //        ticketDTO.AssignedPersonId = ticket.AssignedPersonId;
        //        ticketDTO.SubmitterId = ticket.SubmitterId;
        //        ticketDTO.ProjectName = ticket.ProjectName;

        //        return ticketDTO;
        //    }).ToList();

        //    var personWithTickets = _mapper.Map<PersonDTOWithTickets>(person);

        //    personWithTickets.Tickets = tickets;

        //    return personWithTickets;
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(PersonCreationDTO personCreationDTO)
        {
            var PersonWithTheSameEmailExists = await _personRepository.CheckIfUserWithSameEmailExists(personCreationDTO);

            if (PersonWithTheSameEmailExists)
            {
                return BadRequest($"Ya existe autor con el correo {personCreationDTO.Email}");
            }

            var person = _mapper.Map<Person>(personCreationDTO);

            await _personRepository.SaveToDB(person);

            var PersonDTO = _mapper.Map<PersonDTO>(person);
            return CreatedAtRoute("GetPerson", new {id  = person.Id}, PersonDTO);
        }

        [HttpPut("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> Put(int id, PersonUpdateDTO personUpdateDTO)
        {
            var personDB = await _personRepository.GetPerson(id);
            if (personDB == null)
                return NotFound();

            personDB = _mapper.Map(personUpdateDTO, personDB);

            await _personRepository.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [AllowAnonymous]

        //public async Task<ActionResult> Patch(int id, JsonPatchDocument<PersonPatchDTO> patchDocument)
        //{
        //    if (patchDocument == null)
        //        return BadRequest();

        //    var personDB = await _context.Personnel.FirstOrDefaultAsync(x=>x.Id== id);

        //    if(personDB == null)
        //        return NotFound();

        //    var personDTO = _mapper.Map<PersonPatchDTO>(personDB);

        //    patchDocument.ApplyTo(personDTO, ModelState);

        //    var isValid = TryValidateModel(personDTO);

        //    if(!isValid)
        //        return BadRequest(ModelState);

        //    _mapper.Map(personDTO, personDB);

        //    await _personRepository.SaveChanges();

        //    return NoContent();

        //}
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _personRepository.CheckPersonExists(id);
            if(exists == null) return NotFound();
            _personRepository.DeletePerson(id);
            await _personRepository.SaveChanges();
            return NoContent();
        }
    }
}
