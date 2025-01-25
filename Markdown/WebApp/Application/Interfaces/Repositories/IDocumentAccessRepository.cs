using Core.Models;

namespace Application.Interfaces.Repositories;

public interface IDocumentAccessRepository
{
    Task<Result<bool>> HasAccess(Guid userId, Guid documentId);
    Task<Result> Create(Guid userId, Guid documentId);
    Task<Result> Delete(Guid userId, Guid documentId);
    
    Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId);
}