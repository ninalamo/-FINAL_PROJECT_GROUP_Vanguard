using HelpdeskBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("role_id")]
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    [Column("department_id")]
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public ICollection<Ticket>? CreatedTickets { get; set; }
    public ICollection<Ticket>? AssignedTickets { get; set; }
    public ICollection<Remark>? Remarks { get; set; }
}
