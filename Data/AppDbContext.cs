using Microsoft.EntityFrameworkCore;
using HelpdeskBackend.Models;
using System.Data;

namespace HelpdeskBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Remark> Remarks => Set<Remark>();
    public DbSet<Role> Roles => Set<Role>(); // ? new

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role) // ? new
            .WithMany()
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTickets)
            .HasForeignKey(t => t.CreatedBy);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssignedTo);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Department)
            .WithMany(d => d.Tickets)
            .HasForeignKey(t => t.DepartmentId);

        modelBuilder.Entity<Remark>()
            .HasOne(r => r.User)
            .WithMany(u => u.Remarks)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Remark>()
            .HasOne(r => r.Ticket)
            .WithMany(t => t.Remarks)
            .HasForeignKey(r => r.TicketId);
    }
}
