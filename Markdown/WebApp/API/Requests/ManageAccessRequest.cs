namespace API.Requests;

public class ManageAccessRequest : CustomRequest
{
    public required string UserLogin  { get; set; }
    public required Guid DocumentId { get; set; }
}