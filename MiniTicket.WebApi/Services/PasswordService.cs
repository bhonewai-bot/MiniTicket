using Microsoft.AspNetCore.Identity;
using MiniTicket.Database.AppDbContextModels;

namespace MiniTicket.WebApi.Services;

public class PasswordService
{
    private readonly PasswordHasher<TblUser> _hasher;

    public PasswordService()
    {
        _hasher = new PasswordHasher<TblUser>();
    }

    public string Hash(TblUser user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool Verify(TblUser user, string password, string hash)
    {
        return _hasher.VerifyHashedPassword(user, hash, password) == PasswordVerificationResult.Success;
    }
}