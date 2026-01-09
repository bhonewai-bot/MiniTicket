using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTicket.WebApi.Dtos;
using MiniTicket.WebApi.Services;

namespace MiniTicket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserRequestDto request)
        {
            var result = await _userService.Register(request);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
            
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            return Ok(result.Data);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserRequestDto request)
        {
            var result = await _userService.Login(request);
            
            if (result.IsValidatorError)
                return BadRequest(result.Message);
    
            if (result.IsSystemError)
                return StatusCode(500, result.Message);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = result.Data.SessionExpiredAt
            };

            Response.Cookies.Append("Authorization", result.Data.SessionId, cookieOptions);

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Authorization");
            return Ok(new { message = "Logout successful" });
        }
    }
}
