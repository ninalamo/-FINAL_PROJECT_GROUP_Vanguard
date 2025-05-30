using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpdeskBackend.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<User>? Users { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
