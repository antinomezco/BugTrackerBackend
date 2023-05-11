using AutoMapper;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Repositories;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BugTracker.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPersonRepository _personRepository;

        public TicketService(IMapper mapper, ITicketRepository ticketRepository, IProjectRepository projectRepository, IPersonRepository personRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        public IQueryable<TicketDTO> GetQueryableTicketDetails(int id)
        {
            var tickets = _ticketRepository.GetTicketQueryableList(id);
            var ticketWithDetails = AddDetailsToQueryableTickets(tickets);
            return ticketWithDetails;
            //return tickets;
        }

        public async Task<List<TicketDTO>> GetListTicketDetails(int id)
        {
            var tickets = await _ticketRepository.GetTicketList(id);
            var ticketWithDetails = AddDetailsToListTickets(tickets);
            return ticketWithDetails;
        }

        public async Task<TicketDTOWithDetails> GetTicketDetails(int projectId, int id)
        {
            var tickets = await _ticketRepository.GetTicket(projectId, id);
            var ticketWithDetails = await AddDetailsToTicket(tickets);
            return ticketWithDetails;
        }

        public async Task<List<TicketDTO>> GetTicketsDetails(int id)
        {
            var tickets = await _ticketRepository.GetTickets(id);
            var ticketWithDetails = AddDetailsToListTickets(tickets);
            return ticketWithDetails;
        }

        public async Task<Project> IsProjectExist(int projectId)
        {
            return await _projectRepository.CheckProjectExists(projectId);
        }

        public async Task<Person> IsSubmitterExist(int submitterId)
        {
            return await _personRepository.CheckPersonExists(submitterId);
        }

        public async Task<Ticket> SaveTicket(Ticket ticket)
        {
            await _ticketRepository.SaveTicketToDatabase(ticket);
            return ticket;
        }

        private List<TicketDTO> AddDetailsToListTickets(List<Ticket> ticketData)
        {
            return TicketMap(ticketData).ToList();
        }

        private IQueryable<TicketDTO> AddDetailsToQueryableTickets(IQueryable<Ticket> ticketData)
        {
            return TicketQMap(ticketData).AsQueryable();
        }

        private async Task<TicketDTOWithDetails> AddDetailsToTicket(Ticket ticket)
        {
            if (ticket == null)
                return null;

            var result = _mapper.Map<TicketDTOWithDetails>(ticket);
            var projectdeets = await _projectRepository.GetProjectAsync(ticket.ProjectId);
            var assignedDeets = await _personRepository.GetPerson((int)ticket.AssignedPersonId);
            var submitterDeets = await _personRepository.GetPerson(ticket.SubmitterId);
            if(assignedDeets == null || submitterDeets == null) return null;
            result.Id = ticket.Id;
            result.Title = ticket.Title;
            result.Description = ticket.Description;
            result.ProjectId = ticket.ProjectId;
            result.ProjectName = projectdeets.Name;
            result.SubmitterId = (int)ticket.SubmitterId;
            result.SubmitterPersonName = submitterDeets.Name;
            result.AssignedPersonId = ticket.AssignedPersonId;
            result.AssignedPerson = assignedDeets.Name;
            result.IsResolved = ticket.IsResolved;
            result.CreatedDate = ticket.CreatedDate;
            result.UpdatedDate = ticket.UpdatedDate;


            return result;
        }

        private static IQueryable<TicketDTO> TicketQMap(IQueryable<Ticket> ticketData)
        {
            var tickets = ticketData.Select(ticket =>
                new TicketDTO
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    ProjectId = ticket.Project.Id,
                    ProjectName = ticket.Project.Name,
                    SubmitterId = (int)ticket.SubmitterPerson.Id,
                    SubmitterPersonName = ticket.SubmitterPerson.Name,
                    AssignedPersonId = ticket.AssignedPerson.Id,
                    AssignedPerson = ticket.AssignedPerson.Name,
                    IsResolved = ticket.IsResolved,
                    CreatedDate = ticket.CreatedDate
                }
            );
            return tickets;
        }
        private static IEnumerable<TicketDTO> TicketMap(List<Ticket> ticketData)
        {
            var tickets = ticketData.Select(ticket =>
                new TicketDTO
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    ProjectId = ticket.Project.Id,
                    ProjectName = ticket.Project.Name,
                    SubmitterId = (int)ticket.SubmitterPerson.Id,
                    SubmitterPersonName = ticket.SubmitterPerson.Name,
                    AssignedPersonId = ticket.AssignedPerson.Id,
                    AssignedPerson = ticket.AssignedPerson.Name,
                    IsResolved = ticket.IsResolved,
                    CreatedDate = ticket.CreatedDate
                }
            );
            return tickets;
        }
    }
}
