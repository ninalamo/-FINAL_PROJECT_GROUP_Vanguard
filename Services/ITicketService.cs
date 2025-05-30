using HelpdeskBackend.DTOs;
using HelpdeskBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpdeskBackend.Services
{
    public interface ITicketService
    {
        bool AssignTicket(User user, int ticketId, int assigneeId);
        bool DelegateTicket(User user, int ticketId, int delegateId);
        TicketDto CreateTicket(User user, TicketCreateDto dto);
        TicketDto UpdateTicketStatus(User user, TicketUpdateDto dto);
        TicketDto AddRemark(User user, RemarkDto dto);
        List<TicketDto> GetTickets(User user);
        List<TicketDto> GetTicketsForUser(User user);
        Task<Ticket?> GetTicketById(int id);
        Task<IEnumerable<Remark>> GetRemarksByTicketId(int ticketId);

        // ✅ ADD THIS METHOD
        List<TicketDto> GetAllTickets();
    }
}
