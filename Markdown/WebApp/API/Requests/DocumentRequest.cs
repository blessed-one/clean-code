namespace API.Requests;

public class DocumentRequest : CustomRequest
{
    public string Name { get; set; }
    public Guid DocumentId { get; set; }
}