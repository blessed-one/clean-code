using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Application;
using Core.Models;
using Persistence.Entities;

namespace Persistence.Repositories;

public class UsersRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<Result<List<User>>> GetAll()
    {
        var userEntities = await dbContext.Users
            .AsNoTracking()
            .ToListAsync();
        
        return Result<List<User>>.Success(
            userEntities
                .Select(
                    uEntity => User.Create(uEntity.Id, uEntity.Login, uEntity.PasswordHash))
                .ToList());
    }

    public async Task<Result<User>> GetById(Guid id)
    {
        var userEntity = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        if (userEntity == null)
            return Result<User>.Failure("User not found.");

        var user = User.Create(userEntity.Id, userEntity.Login, userEntity.PasswordHash);
        return Result<User>.Success(user);
    }

    public async Task<Result> Create(Guid userId, string userName, string passwordHash)
    {
        var userEntity = new UserEntity
        {
            Id = userId,
            Login = userName,
            PasswordHash = passwordHash
        };

        try
        {
            await dbContext.Users.AddAsync(userEntity);
            await dbContext.SaveChangesAsync();
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to add user: {ex.Message}");
        }
    }

    public async Task<Result> AddDocument(Guid userId, Guid documentId, string documentName)
    {
        var documentEntity = new DocumentEntity
        {
            Id = documentId,
            Name = documentName,
            AuthorId = userId,
            CreationDateTime = DateTime.Now,
        };

        try
        {
            await dbContext.Documents.AddAsync(documentEntity);
            await dbContext.SaveChangesAsync();
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to add document: {ex.Message}");
        }
    }
}