using Core.Models;

namespace Application.Interfaces.Repositories;

public interface IDocumentRepository
{
    Task<Result<List<Document>>> GetAll();
    Task<Result<Document>> GetById(Guid id);
    Task<Result<List<Document>>> GetByAuthor(string author);
    Task<Result<List<Document>>> GetByAuthorId(Guid authorId);

    Task<Result<Guid>> Create(string documentName, Guid authorId);
    Task<Result> Update(Guid documentId, string documentName);
    Task<Result> Delete(Guid documentId);
}