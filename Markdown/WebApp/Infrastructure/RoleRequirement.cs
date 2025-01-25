using Microsoft.AspNetCore.Authorization;

namespace Infrastructure;

public class RoleRequirement(params string[] roles) : IAuthorizationRequirement
{
    public string[] Roles { get; } = roles;
}
