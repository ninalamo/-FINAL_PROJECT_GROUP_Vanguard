using HelpdeskBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Tickets")]
public class Ticket
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("assigned_to")]
    public int? AssignedTo { get; set; }

    [Column("severity")]
    public string Severity { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    // ✅ Navigation properties — prevent infinite loops
    [JsonIgnore]
    [ForeignKey("CreatedBy")]
    public User Creator { get; set; }

    [JsonIgnore]
    [ForeignKey("AssignedTo")]
    public User Assignee { get; set; }

    [JsonIgnore]
    [ForeignKey("DepartmentId")]
    public Department Department { get; set; }

    [JsonIgnore]
    public ICollection<Remark> Remarks { get; set; }
}
