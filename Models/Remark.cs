using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HelpdeskBackend.Models
{
    [Table("Remarks")]
    public class Remark
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ticket_id")]
        public int TicketId { get; set; }

        [JsonIgnore]
        public Ticket? Ticket { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
