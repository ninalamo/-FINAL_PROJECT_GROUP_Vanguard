using HelpdeskBackend.Data;
using HelpdeskBackend.DTOs;
using HelpdeskBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskBackend.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;

    public TicketService(AppDbContext context)
    {
        _context = context;
    }

    public bool AssignTicket(User user, int ticketId, int assigneeId)
    {
        if (user.Role.Name != "Supervisor")
            throw new UnauthorizedAccessException("Only Supervisors can assign tickets.");

        var ticket = _context.Tickets.Find(ticketId);
        if (ticket == null) return false;

        if (ticket.DepartmentId != user.DepartmentId)
            throw new UnauthorizedAccessException("Supervisors can only assign tickets within their department.");

        ticket.AssignedTo = assigneeId;
        _context.SaveChanges();
        return true;
    }

    public bool DelegateTicket(User user, int ticketId, int newDepartmentId)
    {
        if (user.Role.Name != "Supervisor")
            throw new UnauthorizedAccessException("Only Supervisors can delegate tickets.");

        var ticket = _context.Tickets.Find(ticketId);
        if (ticket == null) return false;

        ticket.DepartmentId = newDepartmentId;
        _context.SaveChanges();
        return true;
    }

    public TicketDto CreateTicket(User user, TicketCreateDto dto)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            DepartmentId = user.DepartmentId,
            Severity = dto.Severity,
            Status = "Open",
            CreatedBy = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedBy = ticket.CreatedBy,
            DepartmentId = ticket.DepartmentId,
            Severity = ticket.Severity,
            Status = ticket.Status
        };
    }

    public TicketDto UpdateTicketStatus(User user, TicketUpdateDto dto)
    {
        var ticket = _context.Tickets.FirstOrDefault(t => t.Id == dto.TicketId);
        if (ticket == null) return null;

        if (ticket.Severity == "Critical" && user.Role.Name != "Supervisor" && ticket.AssignedTo != user.Id)
            throw new UnauthorizedAccessException("Only Supervisors or Assigned users can act on Critical tickets.");

        if (ticket.Severity != "Critical" && user.Role.Name == "Junior Officer" && ticket.AssignedTo != user.Id)
        {
            if (ticket.Severity == "Low" || ticket.Severity == "Medium" || ticket.Severity == "High")
            {
                // Junior Officer can proceed
            }
            else
            {
                throw new UnauthorizedAccessException("Junior Officers can only act on Low–High severity tickets unless assigned.");
            }
        }

        ticket.Status = dto.Status;
        _context.SaveChanges();

        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedBy = ticket.CreatedBy,
            DepartmentId = ticket.DepartmentId,
            Severity = ticket.Severity,
            Status = ticket.Status
        };
    }

    public TicketDto AddRemark(User user, RemarkDto dto)
    {
        var ticket = _context.Tickets.FirstOrDefault(t => t.Id == dto.TicketId);
        if (ticket == null) return null;

        var remark = new Remark
        {
            TicketId = ticket.Id,
            Content = dto.Remark,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.Remarks.Add(remark);
        _context.SaveChanges();

        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedBy = ticket.CreatedBy,
            DepartmentId = ticket.DepartmentId,
            Severity = ticket.Severity,
            Status = ticket.Status
        };
    }

    // ✅ Used for showing all tickets to any logged-in user (e.g. for overview pages)
    public List<TicketDto> GetAllTickets()
    {
        var tickets = _context.Tickets
            .Include(t => t.Remarks)
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .Include(t => t.Department)
            .ToList();

        return tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedBy = t.CreatedBy,
            DepartmentId = t.DepartmentId,
            Severity = t.Severity,
            Status = t.Status
        }).ToList();
    }

    public List<TicketDto> GetTickets(User user)
    {
        var tickets = _context.Tickets
            .Include(t => t.Remarks)
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .Include(t => t.Department)
            .Where(t =>
                t.DepartmentId == user.DepartmentId ||
                (t.Severity == "Critical"))
            .ToList();

        return tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedBy = t.CreatedBy,
            DepartmentId = t.DepartmentId,
            Severity = t.Severity,
            Status = t.Status
        }).ToList();
    }

    public List<TicketDto> GetTicketsForUser(User user)
    {
        var tickets = _context.Tickets
            .Where(t => t.CreatedBy == user.Id || t.AssignedTo == user.Id)
            .ToList();

        return tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedBy = t.CreatedBy,
            DepartmentId = t.DepartmentId,
            Severity = t.Severity,
            Status = t.Status
        }).ToList();
    }

    public async Task<Ticket?> GetTicketById(int id)
    {
        return await _context.Tickets
            .Include(t => t.Remarks)
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Remark>> GetRemarksByTicketId(int ticketId)
    {
        return await _context.Remarks
            .Where(r => r.TicketId == ticketId)
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();
    }
}
