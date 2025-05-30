using Microsoft.AspNetCore.Mvc;
using HelpdeskBackend.DTOs;
using HelpdeskBackend.Models;
using HelpdeskBackend.Services;

namespace HelpdeskBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _authService.ValidateUser(dto);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
