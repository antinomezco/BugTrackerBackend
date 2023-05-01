using AutoMapper;
using BugTracker.DTOs.Ticket;
using BugTracker.Repositories;
using System.Collections.Generic;

namespace BugTracker.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;

        public TicketService(IMapper mapper, ITicketRepository ticketRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        public async Task<List<TicketDTO>> GetListTicketDetails(int id)
        {
            var tickets = await _ticketRepository.GetTicketList(id);
            var ticketWithDetails = AddDetailsToListTickets(tickets);
            return ticketWithDetails;
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
    }
}
