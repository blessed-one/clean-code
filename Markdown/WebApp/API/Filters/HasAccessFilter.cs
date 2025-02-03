using System.Security.Claims;
using API.Requests;
using Application.Interfaces.Services;
using Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol.Plugins;

namespace API.Filters;

public class HasAccessFilter : IAsyncActionFilter
{
    private readonly IDocumentAccessService _accessService;
    private readonly DocumentAccessRoles _requiredRole;

    public HasAccessFilter(IDocumentAccessService accessService, DocumentAccessRoles requiredRole)
    {
        _accessService = accessService;
        _requiredRole = requiredRole;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Guid documentId;

        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        Console.WriteLine("Available Action Arguments:");
        foreach (var key in context.ActionArguments.Keys)
        {
            Console.WriteLine($"Key: {key}, Type: {context.ActionArguments[key]?.GetType().Name}");
        }
        
        if (context.ActionArguments.TryGetValue("documentId", out var paramDocId) && paramDocId is Guid docId)
        {
            documentId = docId;
        }
        else if (!context.ActionArguments.TryGetValue("request", out var requestObj) ||
                 requestObj is not CustomRequest request)
        {
            context.Result = new BadRequestObjectResult("Invalid or missing request data.");
            return;
        }
        else
        {
            documentId = request.DocumentId;
            Console.WriteLine($"Request object: {System.Text.Json.JsonSerializer.Serialize(request)}");
            Console.WriteLine($"Extracted DocumentId: {request.DocumentId}");
        }

        var accessRoleResult = await _accessService.GetAccessRole(userId, documentId);
        if (accessRoleResult.IsFailure)
        {
            context.Result = new BadRequestObjectResult("Access role not found.");
            return;
        }

        Console.WriteLine(
            $"required role: {_requiredRole}, user role: {accessRoleResult.Data}\ndocument ID: {documentId}, user ID: {userId}");

        await next();
    }
}