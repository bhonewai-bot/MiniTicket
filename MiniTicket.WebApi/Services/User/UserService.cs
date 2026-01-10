using Microsoft.EntityFrameworkCore;
using MiniTicket.Database.AppDbContextModels;
using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwordService;

    public UserService(AppDbContext db, PasswordService passwordService)
    {
        _db = db;
        _passwordService = passwordService;
    }

    public async Task<Result<RegisterUserResponseDto>> Register(RegisterUserRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<RegisterUserResponseDto>.ValidationError("Name is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return Result<RegisterUserResponseDto>.ValidationError("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result<RegisterUserResponseDto>.ValidationError("Password is required");
            
            if (request.Password.Length < 6)
                return Result<RegisterUserResponseDto>.ValidationError("Password must be at least 6 characters.");
            
            var email = request.Email.Trim().ToLower();
            
            var exists = await _db.TblUsers
                .AnyAsync(x => x.Email == email);

            if (exists)
                return Result<RegisterUserResponseDto>.ValidationError("Email already exists.");

            var user = new TblUser()
            {
                Name = request.Name.Trim(),
                Email = email,
                Role = UserRole.Admin.ToString(),
                CreatedAt = DateTime.UtcNow,
            };
            
            user.Password = _passwordService.Hash(user, request.Password);

            _db.TblUsers.Add(user);
            await _db.SaveChangesAsync();

            var registeredUser = new RegisterUserResponseDto()
            {
                Name = user.Name,
                Email = user.Email,
                Role = Enum.Parse<UserRole>(user.Role),
            };
            
            return Result<RegisterUserResponseDto>.Success(registeredUser);
            
        }
        catch (Exception ex)
        {
            return Result<RegisterUserResponseDto>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<LoginUserResponseDto>> Login(LoginUserRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return Result<LoginUserResponseDto>.ValidationError("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result<LoginUserResponseDto>.ValidationError("Password is required");
            
            var user = await _db.TblUsers
                .FirstOrDefaultAsync(x => x.Email == request.Email);
            
            if (user is null)
                return Result<LoginUserResponseDto>.ValidationError("Invalid credentials.");

            var valid = _passwordService.Verify(user, request.Password, user.Password);
            if (!valid)
                return Result<LoginUserResponseDto>.ValidationError("Invalid credentials.");

            TblLogin session = new TblLogin()
            {
                UserId = user.UserId,
                SessionId = Guid.NewGuid().ToString(),
                SessionExpiredAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow,
            };
            
            _db.TblLogins.Add(session);
            await _db.SaveChangesAsync();

            var loginUser = new LoginUserResponseDto()
            {
                SessionId = session.SessionId,
                SessionExpiredAt = session.SessionExpiredAt,
                Message = "Login Successful",
            };
            
            return Result<LoginUserResponseDto>.Success(loginUser);
        }
        catch (Exception ex)
        {
            return Result<LoginUserResponseDto>.SystemError(ex.ToString());
        }
    }
}