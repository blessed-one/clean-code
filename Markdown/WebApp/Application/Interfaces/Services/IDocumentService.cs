using Application.Utils;
using Core.Models;

namespace Application.Interfaces.Services;

public interface IDocumentService
{
    Task<Result<List<Document>>> GetAll();
    Task<Result<Document>> GetById(Guid id);
    Task<Result<List<Document>>> GetByAuthorId(Guid authorId);

    Task<Result<Guid>> Create(string documentName, Guid authorId);
    Task<Result> Update(Guid documentId, string documentText);
    Task<Result<string>> GetDocumentContent(Guid documentId);
    Task<Result> Delete(Guid documentId);
}