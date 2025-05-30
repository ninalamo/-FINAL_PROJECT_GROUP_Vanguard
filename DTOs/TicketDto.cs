namespace HelpdeskBackend.DTOs;

public class TicketCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; }
    public string Status { get; set; }
}
public class TicketDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int CreatedBy { get; set; }
    public int DepartmentId { get; set; }
    public string Severity { get; set; }
    public string Status { get; set; } // ✅ Add this
}

public class RemarkDto
{
    
    
        public int TicketId { get; set; }
        public string Remark { get; set; } = string.Empty;
    }



public class TicketUpdateDto
{
    internal int TicketId;

    public string Title { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; }
    public string Status { get; set; }
    public string Department { get; set; }
}


