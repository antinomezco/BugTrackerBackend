using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace BugTracker.Controllers
{
    [Route("api/projects/{projectId:int}/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<List<TicketDTO>>> Get(int projectId)
        {
            var ticketExists = await _context.Projects.AnyAsync(x => x.Id == projectId);

            if (!ticketExists)
                return NotFound();

            var ticketData = await _context.Tickets
                .Where(ticketDB => ticketDB.ProjectId == projectId)
                .Select(ticketDB => new
                {
                    ticket = ticketDB,
                    submitterPersonName = _context.Personnel
                        .Where(person => person.Id == ticketDB.SubmitterPersonId)
                        .Select(person => person.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var tickets = ticketData.Select(ticket => {
                var ticketDTO = _mapper.Map<TicketDTO>(ticket.ticket);
                ticketDTO.SubmitterPersonName = ticket.submitterPersonName;
                return ticketDTO;
            }).ToList();

            return tickets;
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        public async Task<ActionResult<TicketDTOWithDetails>> Get(int projectId, int id)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(x=>x.Id == id);
            if (ticket == null)
                return NotFound();
            var submitterPersonName = await _context.Personnel
                .Where(x => x.Id == ticket.SubmitterPersonId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            var result = _mapper.Map<TicketDTOWithDetails>(ticket);
            result.SubmitterPersonName = submitterPersonName;

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post(int projectId, TicketCreationDTO ticketCreationDTO)
        {
            var ticketExists = await _context.Projects.AnyAsync(x => x.Id == projectId);

            if (!ticketExists)
                return NotFound();

            var submitterPerson = await _context.Personnel.FindAsync(ticketCreationDTO.SubmitterPersonId);

            if (submitterPerson == null)
                return NotFound();

            var ticket = _mapper.Map<Ticket>(ticketCreationDTO);
            ticket.ProjectId = projectId;


            _context.Add(ticket);
            //await _context.SaveChangesAsync();
            //ticket.SubmitterPerson = new TicketPerson { Person = submitterPerson };
            //ticket = _mapper.Map(ticketCreationDTO, ticket);
            await _context.SaveChangesAsync();

            var ticketDTOWithDetails = _mapper.Map<TicketDTOWithDetails>(ticket);
            return CreatedAtRoute("GetTicket", new { projectId = ticket.ProjectId, id = ticket.Id }, ticketDTOWithDetails);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, TicketUpdateDTO ticketUpdateDTO)
        {
            var ticketDB = await _context.Tickets
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ticketDB == null)
                return NotFound();
            if (ticketUpdateDTO.ProjectId != id)
                return BadRequest("Why did you try to update a ticket in a different project?");

            ticketDB = _mapper.Map(ticketUpdateDTO, ticketDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
