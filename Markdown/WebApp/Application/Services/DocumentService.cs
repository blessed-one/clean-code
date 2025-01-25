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
        var repositoryResult = await documentRepository.Create(documentName, authorId);
        
        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message!)
            : Result.Success();
    }

    public Task<Result> Update(Guid documentId, string documentName)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Delete(Guid documentId)
    {
        throw new NotImplementedException();
    }
}