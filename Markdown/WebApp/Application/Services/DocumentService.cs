using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Models;

namespace Application.Services;

public class DocumentService(IDocumentRepository documentRepository) : IDocumentService
{
    public async Task<Result<List<Document>>> GetAll()
    {
        var repositoryResult = await documentRepository.GetAll();

        return repositoryResult.IsFailure
            ? Result<List<Document>>.Failure(repositoryResult.Message!)
            : Result<List<Document>>.Success(repositoryResult.Data!);
    }

    public async Task<Result> Create(string documentName, Guid authorId)
    {
        // TODO add to miro
        var repositoryResult = await documentRepository.Create(documentName, authorId);
        
        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message!)
            : Result.Success();
    }

    public Task<Result> Update(Guid documentId, string documentName)
    {
        // TODO add to miro
        throw new NotImplementedException();
    }

    public async Task<Result> Delete(Guid documentId, Guid userId)
    {
        var docsRepositoryResult = await documentRepository.GetByAuthorId(userId);
        if (docsRepositoryResult.IsFailure)
        {
            return Result.Failure(docsRepositoryResult.Message!);
        }
        var usersDocs = docsRepositoryResult.Data!;
        
        var isInUsersDocs = usersDocs.Any(x => x.Id == documentId);
        if (!isInUsersDocs)
        {
            return Result.Failure($"Document {documentId} does not belongs to user");
        }
        
        var deleteRepositoryResult = await documentRepository.Delete(documentId);
        if (deleteRepositoryResult.IsFailure)
        {
            return Result.Failure(deleteRepositoryResult.Message!);
        }
        
        return Result.Success();
    }
}