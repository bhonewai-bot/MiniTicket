using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTicket.Database.AppDbContextModels;
using MiniTicket.WebApi.Dtos;
using MiniTicket.WebApi.Services.TicketComment;

namespace MiniTicket.WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTicketCommentController : ControllerBase
    {
        private readonly ITicketCommentService _ticketCommentService;

        public AdminTicketCommentController(ITicketCommentService ticketCommentService)
        {
            _ticketCommentService = ticketCommentService;
        }

        private bool CheckAdmin()
        {
            return HttpContext.IsAdmin();
        }

        [HttpPost("{ticketId}/comment")]
        public async Task<IActionResult> ReplyComment(int ticketId, TicketCommentCreateRequestDto request)
        {
            if (!CheckAdmin())
                return Forbid();
            
            if (!HttpContext.Items.TryGetValue("UserId", out var user))
                return Unauthorized();
            
            var adminId = (int)user;

            var role = UserRole.Admin;
            
            var result = await _ticketCommentService.CreateComment(ticketId, adminId, role, request);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
            
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            return Ok(result.Data);
        }
    }
}
