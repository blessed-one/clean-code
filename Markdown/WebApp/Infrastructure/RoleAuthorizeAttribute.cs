using Microsoft.AspNetCore.Authorization;

namespace Infrastructure;

public class RoleAuthorizeAttribute(params string[] roles) : AuthorizeAttribute
{
    private readonly string[] _roles = roles;
}