namespace API.Requests;

public class DocumentIdRequest : CustomRequest
{
    public required Guid DocumentId { get; set; }
}