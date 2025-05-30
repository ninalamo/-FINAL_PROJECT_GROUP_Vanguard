using HelpdeskBackend.Models;

namespace HelpdeskBackend.Data;

public class SeedService
{
    private readonly AppDbContext _context;

    public SeedService(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Departments.Any() || _context.Roles.Any()) return;

        // Seed roles
        var adminRole = new Role { Name = "Admin" };
        var officerRole = new Role { Name = "Officer" };
        _context.Roles.AddRange(adminRole, officerRole);

        // Seed departments
        var itDept = new Department { Name = "IT" };
        var hrDept = new Department { Name = "HR" };
        _context.Departments.AddRange(itDept, hrDept);

        _context.SaveChanges();

        // Create users with RoleId and DepartmentId
        var admin = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            Password = "password",
            RoleId = adminRole.Id,
            DepartmentId = itDept.Id
        };

        var officer = new User
        {
            Username = "officerHR",
            Email = "officer@example.com",
            Password = "password",
            RoleId = officerRole.Id,
            DepartmentId = hrDept.Id
        };

        _context.Users.AddRange(admin, officer);
        _context.SaveChanges();
    }
}
