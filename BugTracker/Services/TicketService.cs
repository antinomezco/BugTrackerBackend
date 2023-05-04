using AutoMapper;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;
using BugTracker.Repositories;
using System.Collections.Generic;

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

        public async Task<List<TicketDTO>> GetListTicketDetails(int id)
        {
            var tickets = await _ticketRepository.GetTicketList(id);
            var ticketWithDetails = AddDetailsToListTickets(tickets);
            return ticketWithDetails;
        }

        public async Task<TicketDTOWithDetails> GetTicketDetails(int projectId, int id)
        {
            var tickets = await _ticketRepository.GetTicket(projectId,id);
            var ticketWithDetails = AddDetailsToTicket(tickets);
            return ticketWithDetails;
        }

        public async Task<Project> IsProjectExist(int projectId)
        {
            return await _projectRepository.CheckProjectExists(projectId);
        }

        public async Task<Person> IsSubmitterExist(int submitterPersonId)
        {
            return await _personRepository.CheckPersonExists(submitterPersonId);
        }

        public async Task<Ticket> SaveTicket(Ticket ticket)
        {
            await _ticketRepository.SaveTicketToDatabase(ticket);
            return ticket;
        }

        private List<TicketDTO> AddDetailsToListTickets(List<TicketInfoDTO> ticketData)
        {
            var tickets = ticketData.Select(ticket => {
                var ticketDTO = _mapper.Map<TicketDTO>(ticket.Ticket);
                ticketDTO.SubmitterPersonName = ticket.SubmitterPersonName;
                ticketDTO.AssignedPerson = ticket.AssignedPersonName;
                ticketDTO.ProjectName = ticket.ProjectName;

                return ticketDTO;
            }).ToList();

            return tickets;
        }

        private TicketDTOWithDetails AddDetailsToTicket(TicketInfoDTO ticket)
        {
            if (ticket == null)
                return null;

            var result = _mapper.Map<TicketDTOWithDetails>(ticket.Ticket);
            result.ProjectName = ticket.ProjectName;
            result.SubmitterPersonName = ticket.SubmitterPersonName;
            result.AssignedPerson = ticket.AssignedPersonName;

            return result;
        }
    }
}
