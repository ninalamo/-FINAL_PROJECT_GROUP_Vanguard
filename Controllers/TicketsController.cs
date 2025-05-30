using System.Security.Claims;
using HelpdeskBackend.DTOs;
using HelpdeskBackend.Services;
using HelpdeskBackend.Data;
using HelpdeskBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly AppDbContext _context;

    public TicketsController(ITicketService ticketService, AppDbContext context)
    {
        _ticketService = ticketService;
        _context = context;
    }

    private User? GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    // 🔐 Get tickets for the logged-in user
    [HttpGet]
    public IActionResult GetMyTickets()
    {
        var user = GetCurrentUser();
        if (user == null) return Unauthorized();

        var tickets = _ticketService.GetTicketsForUser(user);
        return Ok(tickets);
    }

    // ✅ Get all tickets (for Admin/Manager roles)
    [HttpGet("all")]
    public IActionResult GetAllTickets()
    {
        var user = GetCurrentUser();
        if (user == null) return Unauthorized();

        var tickets = _ticketService.GetAllTickets();
        return Ok(tickets);
    }

    // 🔐 Create new ticket
    [HttpPost]
    public IActionResult CreateTicket(TicketCreateDto dto)
    {
        var user = GetCurrentUser();
        if (user == null) return Unauthorized();

        var ticket = _ticketService.CreateTicket(user, dto);
        return Ok(ticket);
    }

    // ✅ Public: Get ticket by ID
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Ticket>> GetTicketById(int id)
    {
        var ticket = await _ticketService.GetTicketById(id);
        if (ticket == null)
            return NotFound(new { message = $"Ticket with ID {id} not found." });

        return Ok(ticket);
    }

    // 🔐 Update ticket details and status (DTO-based)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketUpdateDto dto)
    {
        var user = GetCurrentUser();
        if (user == null) return Unauthorized();

        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket not found" });

        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Severity = dto.Severity;
        ticket.Status = dto.Status;
        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Severity = dto.Severity;
        ticket.Status = dto.Status;

        // Convert department name (string) to actual Department entity
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Name == dto.Department);

        if (department == null)
            return BadRequest(new { message = "Invalid department name" });

        ticket.Department = department;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Ticket updated successfully" });
    }
}
