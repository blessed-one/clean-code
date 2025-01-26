using System.Security.Claims;
using API.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class AccessFilter(IDocumentAccessService accessService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!context.ActionArguments.TryGetValue("request", out var requestObj) || requestObj is not CustomRequest request)
        {
            context.Result = new BadRequestObjectResult("Invalid or missing request data.");
            return;
        }

        var documentId = request.DocumentId;

        var hasAccessResult = await accessService.ValidateAccess(userId, documentId);
        if (hasAccessResult.IsFailure || !hasAccessResult.Data)
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}