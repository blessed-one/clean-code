using Core.Models;

namespace Application.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}