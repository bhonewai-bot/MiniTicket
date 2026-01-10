using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi;

public static class DevCode
{
    public static int GetUserId(this HttpContext context)
    {
        return (int)context.Items["UserId"]!;
    } 
        
    public static bool IsAdmin(this HttpContext context)
    {
        return context.Items.TryGetValue("Role", out var role) &&
               role is UserRole r &&
               r == UserRole.Admin;
    }
}