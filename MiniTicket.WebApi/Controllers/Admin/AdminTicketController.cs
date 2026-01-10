using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTicket.WebApi.Dtos;
using MiniTicket.WebApi.Services;

namespace MiniTicket.WebApi.Controllers.Admin
{
    [Route("api/admin/Tickets")]
    [ApiController]
    public class AdminTicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public AdminTicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private bool CheckAdmin()
        {
            return HttpContext.IsAdmin();
        }

        [HttpGet("{pageNo}/{pageSize}")]
        public async Task<IActionResult> GetTickets(int pageNo, int pageSize)
        {
            if (!CheckAdmin())
                return Forbid();
            
            var result = await _ticketService.GetTickets(pageNo, pageSize);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
            
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            return Ok(result.Data);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTicket(int ticketId)
        {
            if (!CheckAdmin())
                return Forbid();
            
            var result = await _ticketService.GetTicket(ticketId);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
            
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            return Ok(result.Data);
        }
        
        [HttpPatch("{ticketId}/status")]
        public async Task<IActionResult> UpdateTicketStatus(int ticketId, [FromBody] TicketUpdateStatusRequestDto request)
        {
            if (!CheckAdmin())
                return Forbid();
            
            var result = await _ticketService.UpdateTicketStatus(ticketId, request);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
            
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            return Ok(result.Data);
        }
    }
}
