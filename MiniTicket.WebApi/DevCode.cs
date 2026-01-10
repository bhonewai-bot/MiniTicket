namespace MiniTicket.WebApi;

public static class DevCode
{
    public static int GetUserId(this HttpContext context)
    {
        return (int)context.Items["UserId"]!;
    } 
        
    public static bool IsAdmin(this HttpContext context)
    {
        return context.Items["Role"]?.ToString() == "Admin";
    }
}