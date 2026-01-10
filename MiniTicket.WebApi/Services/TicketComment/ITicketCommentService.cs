using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services.TicketComment;

public interface ITicketCommentService
{
    // Task<List<TicketCommentResponseDto>> GetTicketComments(int ticketId, int pageNo, int pageSize);
    Task<Result<TicketCommentResponseDto>> CreateComment(int ticketId, int userId, UserRole role, TicketCommentCreateRequestDto request);
}