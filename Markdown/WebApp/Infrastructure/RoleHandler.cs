using System.Security.Claims;
using Core.Models;

namespace Infrastructure;

using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

public class RoleHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var roleClaim = context.User.FindAll("role").Select(claim => claim.Value);

        if (requirement.Roles.Any(role => roleClaim.Contains(role)))
        {
            context.Succeed(requirement);
            Console.WriteLine(1);
        }

        return Task.CompletedTask;
    }
}
