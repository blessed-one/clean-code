namespace Persistence.Entities;

public class DocumentEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Guid AuthorId { get; set; }
    public UserEntity Author { get; set; }
    
    public DateTime CreationDateTime { get; set; }
}