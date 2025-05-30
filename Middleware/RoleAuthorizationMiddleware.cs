using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HelpdeskBackend.Middleware;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RoleAuthorizationMiddleware> _logger;

    public RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
            var deptId = context.User.FindFirst("DepartmentId")?.Value;

            if (!string.IsNullOrEmpty(role))
            {
                context.Items["UserRole"] = role;
                _logger.LogDebug("User role set: {Role}", role);
            }

            if (!string.IsNullOrEmpty(deptId))
            {
                context.Items["DepartmentId"] = deptId;
                _logger.LogDebug("DepartmentId set: {DepartmentId}", deptId);
            }
        }
        else
        {
            _logger.LogDebug("Request is unauthenticated.");
        }

        await _next(context);
    }
}
