namespace Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login  { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;

    public static User Create(Guid userId, string login, string password) =>
        new()
        {
            Id = userId,
            Login = login,
            Password = password
        };
}