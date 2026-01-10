using Microsoft.EntityFrameworkCore;
using MiniTicket.Database.AppDbContextModels;

namespace MiniTicket.WebApi.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var path = context.Request.Path.Value!.ToLower();
            if (_allowUrlList.Contains(path))
            {
                await _next(context);
                return;
            }
            
            if (!context.Request.Cookies.TryGetValue("Authorization", out var sessionId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var db = context.RequestServices.GetRequiredService<AppDbContext>();
            
            var login = await db.TblLogins
                .FirstOrDefaultAsync(x => 
                    x.SessionId == sessionId &&
                    x.SessionExpiredAt > DateTime.UtcNow);

            if (login is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            
            var user = await db.TblUsers
                .FirstOrDefaultAsync(x => x.UserId == login.UserId);

            if (user is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            context.Items["UserId"] = login.UserId;
            context.Items["Role"] = user.Role;
            
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthMiddleware failed");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Internal server error");
        }
    }

    private string[] _allowUrlList =
    {
        "/api/user/register",
        "/api/user/login",
        "/swagger",
        "/swagger/index.html"
    };
}