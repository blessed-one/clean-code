using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentsRepository
{
    private readonly AppDbContext _dbContext;

    public DocumentsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<DocumentEntity>> GetAll()
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DocumentEntity?> GetById(Guid id)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Id == id);
    }

    public async Task<List<DocumentEntity>> GetByAuthor(string author)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .Where(doc => doc.Author.Equals(author))
            .ToListAsync();
    }

    public async Task<List<DocumentEntity>> GetByPage(int page, int pageSize)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task Add(Guid documentId, string documentName, Guid authorId)
    {
        var documentEntity = new DocumentEntity
        {
            Id = documentId,
            Name = documentName,
            AuthorId = authorId,
            CreationDateTime = DateTime.Now,
        };
        
        await _dbContext.Documents.AddAsync(documentEntity);
        await _dbContext.SaveChangesAsync();
    } 
    
    public async Task Update(Guid documentId, string documentName)
    {
        await _dbContext.Documents
            .Where(doc => doc.Id == documentId)
            .ExecuteUpdateAsync(doc => doc
                .SetProperty(document => document.Name, documentName));
    } 
    
    public async Task Delete(Guid documentId)
    {
        await _dbContext.Documents
            .Where(doc => doc.Id == documentId)
            .ExecuteDeleteAsync();
    } 
}

public class UsersRepository
{
    private readonly AppDbContext _dbContext;

    public UsersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserEntity>> GetAll()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserEntity?> GetById(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task Add(Guid userId, string userName, string passwordHash)
    {
        var userEntity = new UserEntity
        {
            Id = userId,
            Login = userName,
            Password = passwordHash
        };
        
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddDocument(Guid userId, Guid documentId, string documentName)
    {
        var documentEntity = new DocumentEntity
        {
            Id = documentId,
            Name = documentName,
            AuthorId = userId,
            CreationDateTime = DateTime.Now,
        };
        
        await _dbContext.Documents.AddAsync(documentEntity);
        await _dbContext.SaveChangesAsync();
    }
}