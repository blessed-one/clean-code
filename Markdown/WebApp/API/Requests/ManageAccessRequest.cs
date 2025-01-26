namespace API.Requests;

public class ManageAccessRequest : CustomRequest
{
    public required Guid UserId  { get; set; }
    public required Guid DocumentId { get; set; }
}