using HelpdeskBackend.Models;
using HelpdeskBackend.DTOs;

namespace HelpdeskBackend.Services
{
    public interface IAuthService
    {
        User? ValidateUser(LoginDto dto);
        string GenerateToken(User user);
    }
}
