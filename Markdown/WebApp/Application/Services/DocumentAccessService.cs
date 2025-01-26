using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Utils;
using Core.Models;

namespace Application.Services;

public class DocumentAccessService(IDocumentAccessRepository accessRepository) : IDocumentAccessService
{
    public async Task<Result> AddAccess(Guid userId, Guid documentId)
    {
        var repositoryResult = await accessRepository.Create(userId, documentId);

        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message!)
            : Result.Success();
    }

    public async Task<Result> DeleteAccess(Guid userId, Guid documentId)
    {
        var repositoryResult = await accessRepository.Delete(userId, documentId);

        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message!)
            : Result.Success();
    }

    public async Task<Result<bool>> ValidateAccess(Guid userId, Guid documentId)
    {
        var repositoryResult = await accessRepository.HasAccess(userId, documentId);

        return repositoryResult.IsFailure
            ? Result<bool>.Failure(repositoryResult.Message!)
            : Result<bool>.Success(repositoryResult.Data!);
    }

    public async Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId)
    {
        var repositoryResult = await accessRepository.GetUsersAccesses(userId);

        return repositoryResult.IsFailure
            ? Result<List<DocumentAccess>>.Failure(repositoryResult.Message!)
            : Result<List<DocumentAccess>>.Success(repositoryResult.Data!);
    }
}