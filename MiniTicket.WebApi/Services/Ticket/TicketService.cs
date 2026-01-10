using Microsoft.EntityFrameworkCore;
using MiniTicket.Database.AppDbContextModels;
using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _db;

    public TicketService(AppDbContext db)
    {
        _db = db;
    }
    
    // User
    public async Task<Result<List<TicketResponseDto>>> GetTickets(int pageNo, int pageSize, int userId)
    {
        try
        {
            if (pageNo <= 0)
                return Result<List<TicketResponseDto>>.ValidationError("Page number must be greater than zero");
            
            if (pageSize <= 0)
                return Result<List<TicketResponseDto>>.ValidationError("Page size must be greater than zero");

            var tickets = await _db.TblTickets
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.TicketId)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TicketResponseDto()
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    Description = x.Description,
                    Status = Enum.Parse<TicketStatus>(x.Status),
                    UserId = x.UserId
                })
                .ToListAsync();
            
            return Result<List<TicketResponseDto>>.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result<List<TicketResponseDto>>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<TicketResponseDto>> GetTicket(int ticketId, int userId)
    {
        try
        {
            var ticket = await _db.TblTickets
                .AsNoTracking()
                .Where(x => x.TicketId == ticketId && x.UserId == userId)
                .Select(x => new TicketResponseDto()
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    Description = x.Description,
                    Status = Enum.Parse<TicketStatus>(x.Status),
                    UserId = x.UserId
                })
                .FirstOrDefaultAsync();
            
            if (ticket is null)
                return Result<TicketResponseDto>.ValidationError("Ticket not found");
            
            return Result<TicketResponseDto>.Success(ticket);
        }
        catch (Exception ex)
        {
            return Result<TicketResponseDto>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<TicketResponseDto>> CreateTicket(TicketCreateRequestDto request, int userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return Result<TicketResponseDto>.ValidationError("Title is required");
            
            if (string.IsNullOrWhiteSpace(request.Description))
                return Result<TicketResponseDto>.ValidationError("Description is required");

            var ticket = new TblTicket()
            {
                Title = request.Title,
                Description = request.Description,
                Status = TicketStatus.Open.ToString(),
                UserId = userId
            };
            
            _db.Add(ticket);
            await _db.SaveChangesAsync();

            var response = new TicketResponseDto()
            {
                TicketId = ticket.TicketId,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = Enum.Parse<TicketStatus>(ticket.Status),
                UserId = ticket.UserId
            };
            
            return Result<TicketResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<TicketResponseDto>.SystemError(ex.ToString());
        }
    }
    
    // Admin
    public async Task<Result<List<TicketResponseDto>>> GetTickets(int pageNo, int pageSize)
    {
        try
        {
            if (pageNo <= 0)
                return Result<List<TicketResponseDto>>.ValidationError("Page number must be greater than zero");
            
            if (pageSize <= 0)
                return Result<List<TicketResponseDto>>.ValidationError("Page size must be greater than zero");

            var tickets = await _db.TblTickets
                .AsNoTracking()
                .OrderByDescending(x => x.TicketId)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TicketResponseDto()
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    Description = x.Description,
                    Status = Enum.Parse<TicketStatus>(x.Status),
                    UserId = x.UserId
                })
                .ToListAsync();
            
            return Result<List<TicketResponseDto>>.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result<List<TicketResponseDto>>.SystemError(ex.ToString());
        }
    }
    
    public async Task<Result<TicketResponseDto>> GetTicket(int ticketId)
    {
        try
        {
            var ticket = await _db.TblTickets
                .AsNoTracking()
                .Where(x => x.TicketId == ticketId)
                .Select(x => new TicketResponseDto()
                {
                    TicketId = x.TicketId,
                    Title = x.Title,
                    Description = x.Description,
                    Status = Enum.Parse<TicketStatus>(x.Status),
                    UserId = x.UserId
                })
                .FirstOrDefaultAsync();
            
            if (ticket is null)
                return Result<TicketResponseDto>.ValidationError("Ticket not found");
            
            return Result<TicketResponseDto>.Success(ticket);
        }
        catch (Exception ex)
        {
            return Result<TicketResponseDto>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<TicketResponseDto>> UpdateTicketStatus(int ticketId, TicketUpdateStatusRequestDto request)
    {
        try
        {
            if (!Enum.IsDefined(typeof(TicketStatus), request.Status) || request.Status == TicketStatus.None)
                return Result<TicketResponseDto>.ValidationError("Invalid status");

            var ticket = await _db.TblTickets
                .FirstOrDefaultAsync(x => x.TicketId == ticketId);
            
            if (ticket is null)
                return Result<TicketResponseDto>.ValidationError("Ticket not found");
            
            ticket.Status = request.Status.ToString();
            ticket.UpdatedAt = DateTime.UtcNow;
            
            await _db.SaveChangesAsync();
            
            var response = new TicketResponseDto
            {
                TicketId = ticket.TicketId,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = request.Status,
                UserId = ticket.UserId
            };
            
            return Result<TicketResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<TicketResponseDto>.SystemError(ex.ToString());
        }
    }
}