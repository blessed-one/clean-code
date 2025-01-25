using Core.Models;

namespace Application.Interfaces.Services;

public interface IDocumentAccessService
{
    Task<Result> AddAccess(Guid userId, Guid documentId);
    Task<Result> DeleteAccess(Guid userId, Guid documentId);
    Task<Result<bool>> ValidateAccess(Guid userId, Guid documentId, string access);
    
    Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId);
}