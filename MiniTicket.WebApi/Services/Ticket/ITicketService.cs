using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services;

public interface ITicketService
{
    Task<Result<TicketResponseDto>> CreateTicket(TicketCreateRequestDto request);
}