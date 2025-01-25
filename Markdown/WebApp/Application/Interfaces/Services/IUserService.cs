namespace Application.Interfaces.Services;

public interface IUserService
{
    // Task<Result<User>> GetById(Guid id);
    Task<Result> Register(string userName, string passwordHash);
    Task<Result<string>> Login(string userName, string passwordHash);
}