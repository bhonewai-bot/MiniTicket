using Microsoft.EntityFrameworkCore;
using MiniTicket.Database.AppDbContextModels;
using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services.TicketComment;

public class TicketCommentService : ITicketCommentService
{
    private readonly AppDbContext _db;

    public TicketCommentService(AppDbContext db)
    {
        _db = db;
    }
    
    // User
    public async Task<Result<TicketCommentResponseDto>> CreateComment(int ticketId, int userId, UserRole role, TicketCommentCreateRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return Result<TicketCommentResponseDto>.ValidationError("Message is required");

            var ticket = await _db.TblTickets
                .FirstOrDefaultAsync(x => x.TicketId == ticketId);

            if (ticket is null)
                return Result<TicketCommentResponseDto>.ValidationError("Ticket not found");

            if (role == UserRole.User && ticket.UserId != userId)
                return Result<TicketCommentResponseDto>.ValidationError("You are not allowed to comment on this ticket");

            var comment = new TblTicketComment()
            {
                TicketId = ticketId,
                UserId = userId,
                AuthorRole = role.ToString(),
                Message = request.Message,
            };
            
            _db.Add(comment);
            await _db.SaveChangesAsync();

            var response = new TicketCommentResponseDto()
            {
                TicketCommentId = comment.TicketCommentId,
                TicketId = comment.TicketId,
                AuthorRole = Enum.Parse<UserRole>(comment.AuthorRole),
                UserId = comment.UserId,
                Message = comment.Message,
            };
            
            return Result<TicketCommentResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<TicketCommentResponseDto>.SystemError(ex.ToString());
        }
    }
}