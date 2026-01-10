namespace MiniTicket.WebApi.Dtos;

public class TicketResponseDto
{
    public int TicketId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public int UserId { get; set; }
}

public class TicketCreateRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
}

public class TicketUpdateStatusRequestDto
{
    public TicketStatus Status { get; set; }
}

public enum TicketStatus
{
    None,
    Open,
    InProgress,
    Resolved,
    Closed
}