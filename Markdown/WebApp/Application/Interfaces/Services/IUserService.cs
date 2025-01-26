using Application.Utils;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<bool>> ExistById(Guid id);
    Task<Result> Register(string userName, string passwordHash);
    Task<Result<string>> Login(string userName, string passwordHash);
}