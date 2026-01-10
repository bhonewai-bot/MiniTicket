using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTicket.WebApi.Dtos;
using MiniTicket.WebApi.Services.TicketComment;

namespace MiniTicket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCommentController : ControllerBase
    {
        private readonly ITicketCommentService _ticketCommentService;

        public TicketCommentController(ITicketCommentService ticketCommentService)
        {
            _ticketCommentService = ticketCommentService;
        }

        [HttpPost("{ticketId}/comment")]
        public async Task<IActionResult> CreateComment(int ticketId, TicketCommentCreateRequestDto request)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var user))
                return Unauthorized();

            int userId = (int)user;
            var role = UserRole.User;

            var result = await _ticketCommentService.CreateComment(ticketId, userId, role, request);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
            
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            return Ok(result.Data);
        }
    }
}
