namespace API.Requests;

public class ManageAccessRequest
{
    public required Guid UserId  { get; set; }
    public required Guid DocumentId { get; set; }
}