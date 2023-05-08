using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Migrations;
using BugTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(ApplicationDbContext context, ITicketService ticketService,IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        [HttpGet]
        public async Task<ActionResult<List<TicketDTO>>> Get(int projectId)
        {
            var tickets = await _ticketService.GetListTicketDetails(projectId);

            return tickets;
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        public async Task<ActionResult<TicketDTOWithDetails>> Get(int projectId, int id)
        {
            var ticket = await _ticketService.GetTicketDetails(projectId, id);

            if (ticket == null)
                return NotFound();

            return ticket;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int projectId, TicketCreationDTO ticketCreationDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var submitter = await _userManager.FindByEmailAsync(email);
            var submitterId = submitter.Id;
            var projectExists = await _ticketService.IsProjectExist(projectId);

            if (projectExists == null)
                return NotFound();

            var submitterPerson = await _ticketService.IsSubmitterExist(ticketCreationDTO.SubmitterPersonId);

            if (submitterPerson == null)
                return NotFound();

            var ticket = _mapper.Map<Ticket>(ticketCreationDTO);
            ticket.ProjectId = projectId;

            await _ticketService.SaveTicket(ticket);            

            var ticketDTOWithDetails = _mapper.Map<TicketDTOWithDetails>(ticket);
            ticketDTOWithDetails.ProjectName = projectExists.Name;
            ticketDTOWithDetails.SubmitterPersonName = submitterPerson.Name;
            ticketDTOWithDetails.SubmitterPersonId = submitterId;
            //hace falta cambiar el tiket entity para que use strings
            return CreatedAtRoute("GetTicket", new { projectId = ticket.ProjectId, id = ticket.Id }, ticketDTOWithDetails);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, int projectId, TicketUpdateDTO ticketUpdateDTO)
        {

            var ticketDB = await _context.Tickets
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ticketDB == null)
                return NotFound();

            ticketDB = _mapper.Map(ticketUpdateDTO, ticketDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
