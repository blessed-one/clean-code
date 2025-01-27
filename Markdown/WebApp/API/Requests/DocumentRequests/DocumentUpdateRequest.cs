namespace API.Requests;

public class DocumentUpdateRequest : CustomRequest
{
    public required Guid DocumentId { get; set; }
    public required string Text { get; set; }
}