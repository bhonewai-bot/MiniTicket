namespace MiniTicket.WebApi.Dtos;

public class RegisterUserRequestDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserResponseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}

public class LoginUserRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserResponseDto
{
    public string SessionId { get; set; }
    public DateTime SessionExpiredAt { get; set; }
    public string Message { get; set; }
}

public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}

public enum UserRole
{
    None,
    Admin,
    User
}