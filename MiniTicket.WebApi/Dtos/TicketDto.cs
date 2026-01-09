namespace MiniTicket.WebApi.Dtos;

public class TicketResponseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public int UserId { get; set; }
}

public class TicketCreateRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
}