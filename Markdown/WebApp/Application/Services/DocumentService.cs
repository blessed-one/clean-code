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
        var authorsDocsResult = await documentRepository.GetByAuthorId(authorId);
        if (authorsDocsResult.IsFailure)
        {
            return Result<Guid>.Failure(authorsDocsResult.Message!);
        }

        var authorsDocs = authorsDocsResult.Data;
        if (authorsDocs != null && authorsDocs
                .Any(doc => doc.Name == documentName))
        {
            return Result<Guid>.Failure("Document with such name already exists");
        }

        var createResult = await documentRepository.Create(documentName, authorId);

        return createResult.IsFailure
            ? Result<Guid>.Failure(createResult.Message!)
            : Result<Guid>.Success(createResult.Data);
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
            return Result.Failure(deleteRepositoryResult.Message);
        }

        return Result.Success();
    }
}