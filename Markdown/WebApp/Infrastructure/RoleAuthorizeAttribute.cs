using Microsoft.AspNetCore.Authorization;

namespace Infrastructure;

public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles)
    {
        List<string> tempRoles = [];
        tempRoles.AddRange(roles);
        if (!tempRoles.Contains("admin"))
        {
            tempRoles.Add("admin");
        }

        Roles = string.Join(",", tempRoles);
    }
}