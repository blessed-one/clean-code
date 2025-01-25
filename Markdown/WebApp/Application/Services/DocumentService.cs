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

    public async Task<Result<Guid>> Create(string documentName, Guid authorId)
    {
        // TODO add to miro
        // TODO check same name
        var repositoryResult = await documentRepository.Create(documentName, authorId);
        
        return repositoryResult.IsFailure
            ? Result<Guid>.Failure(repositoryResult.Message!)
            : Result<Guid>.Success(repositoryResult.Data);
    }

    public Task<Result> Update(Guid documentId, string documentName)
    {
        // TODO add to miro
        throw new NotImplementedException();
    }

    public async Task<Result> Delete(Guid documentId)
    {
        var deleteRepositoryResult = await documentRepository.Delete(documentId);
        if (deleteRepositoryResult.IsFailure)
        {
            return Result.Failure(deleteRepositoryResult.Message!);
        }
        
        return Result.Success();
    }
}