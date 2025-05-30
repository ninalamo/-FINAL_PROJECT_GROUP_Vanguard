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
public class RemarksController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly AppDbContext _context;

    public RemarksController(ITicketService ticketService, AppDbContext context)
    {
        _ticketService = ticketService;
        _context = context;
    }

    private User? GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    // ✅ Requires authentication
    [HttpPost]
    public IActionResult AddRemark(RemarkDto dto)
    {
        var user = GetCurrentUser();
        if (user == null) return Unauthorized();

        _ticketService.AddRemark(user, dto); // assumed to be a void method
        return Ok(new { message = "Remark added successfully." });
    }

    // ✅ Anonymous access for frontend TicketDetailsPage.jsx
    [HttpGet("{ticketId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Remark>>> GetRemarks(int ticketId)
    {
        var remarks = await _ticketService.GetRemarksByTicketId(ticketId);
        return Ok(remarks);
    }
}
