using Application;
using Application.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentAccessRepository(AppDbContext dbContext) : IDocumentAccessRepository
{
    public async Task<Result<bool>> HasAccess(Guid userId, Guid documentId)
    {
        var hasAccess = await dbContext.DocumentAccesses
            .AnyAsync(access => access.UserId == userId && access.DocumentId == documentId);

        return Result<bool>.Success(hasAccess);
    }

    public async Task<Result> Create(Guid userId, Guid documentId)
    {
        var exists = await dbContext.DocumentAccesses
            .AnyAsync(access => access.UserId == userId && access.DocumentId == documentId);

        if (exists)
        {
            return Result.Failure("Access already exists for the given user and document.");
        }

        var documentAccess = new DocumentAccessEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DocumentId = documentId
        };
        
        try
        {
            await dbContext.DocumentAccesses.AddAsync(documentAccess);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create document access: {ex.Message}");
        }
    }

    public async Task<Result> Delete(Guid userId, Guid documentId)
    {
        try
        {
            await dbContext.DocumentAccesses
                .Where(access => access.UserId == userId && access.DocumentId == documentId)
                .ExecuteDeleteAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete document access: {ex.Message}");
        }
    }

    public async Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId)
    {
        try
        {
            var accessibleDocuments = await dbContext.DocumentAccesses
                .Where(access => access.UserId == userId)
                .Select(access => DocumentAccess.Create(access.Id, access.UserId, access.DocumentId))
                .ToListAsync();

            return Result<List<DocumentAccess>>.Success(accessibleDocuments);
        }
        catch (Exception ex)
        {
            return Result<List<DocumentAccess>>.Failure($"Failed to get user's accesses: {ex.Message}");
        }
    }
}