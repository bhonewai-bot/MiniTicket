using MiniTicket.WebApi.Dtos;

namespace MiniTicket.WebApi.Services;

public interface IUserService
{
    Task<Result<RegisterUserResponseDto>> Register(RegisterUserRequestDto request);
    Task<Result<LoginUserResponseDto>> Login(LoginUserRequestDto request);
}