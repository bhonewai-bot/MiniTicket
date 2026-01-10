namespace MiniTicket.WebApi.Dtos;

public class TicketCommentResponseDto
{
    public int TicketCommentId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public UserRole AuthorRole { get; set; }
    public string Message { get; set; }
}

public class TicketCommentCreateRequestDto
{
    public string Message { get; set; }
}