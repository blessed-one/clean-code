using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class UserService(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IJwtProvider jwtProvider)
    : IUserService
{
    public async Task<Result> Register(string username, string password)
    {
        var hashedPassword = passwordHasher.Generate(password);

        var repositoryResult = await userRepository.Create(Guid.NewGuid(), username, hashedPassword);

        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message)
            : Result.Success();
    }

    public async Task<Result<string>> Login(string userName, string passwordHash)
    {
        var repositoryResult = await userRepository.GetByLogin(userName);

        if (repositoryResult.IsFailure)
        {
            return Result<string>.Failure(repositoryResult.Message!);
        }

        var user = repositoryResult.Data;
        var verificationResult = passwordHasher.Verify(passwordHash, user!.Password);
        if (!verificationResult)
        {
            return Result<string>.Failure("Неверный пароль");
        }

        var token = jwtProvider.GenerateToken(user);

        return Result<string>.Success(token);
    }
}