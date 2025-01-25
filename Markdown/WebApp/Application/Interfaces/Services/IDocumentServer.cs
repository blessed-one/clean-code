using Core.Models;

namespace Application.Interfaces.Services;

public interface IDocumentService
{
    Task<Result<List<Document>>> GetAll();
    // Task<Result<Document>> GetById(Guid id);
    // Task<Result<List<Document>>> GetByAuthor(string author);
    // Task<Result<List<Document>>> GetByPage(int page, int pageSize);

    Task<Result> Create(string documentName, Guid authorId);
    Task<Result> Update(Guid documentId, string documentName);
    Task<Result> Delete(Guid documentId, Guid authorId);
}