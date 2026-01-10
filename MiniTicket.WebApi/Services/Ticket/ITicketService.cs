using Microsoft.AspNetCore.Mvc;
using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services;

public interface ITicketService
{
    // User
    Task<Result<List<TicketResponseDto>>> GetTickets(int pageNo, int pageSize, int userId);
    Task<Result<TicketResponseDto>> GetTicket(int ticketId, int userId);
    Task<Result<TicketResponseDto>> CreateTicket(TicketCreateRequestDto request, int userId);
    
    // Admin
    Task<Result<List<TicketResponseDto>>> GetTickets(int pageNo, int pageSize);
    Task<Result<TicketResponseDto>> GetTicket(int ticketId);
    Task<Result<TicketResponseDto>> UpdateTicketStatus(int ticketId, TicketUpdateStatusRequestDto request);
}