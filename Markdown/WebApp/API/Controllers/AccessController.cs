using System.Security.Claims;
using API.Requests;
using Application.Interfaces.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessController(IDocumentAccessService accessService) : ControllerBase
{
    private async Task<bool> CheckClientAccess(Guid documentId)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out var userId);
        var hasAccessResult = await accessService.ValidateAccess(userId, documentId);

        if (hasAccessResult.IsFailure)
        {
            throw new Exception(hasAccessResult.Message);
        }
        
        return hasAccessResult.Data;
    }
    
    [RoleAuthorize("user")]
    [HttpGet("Give")]
    public async Task<IActionResult> Give([FromQuery] ManageAccessRequest request)
    {
        try
        {
            if (!await CheckClientAccess(request.DocumentId))
            {
                return Forbid();
            }
            
            var addAccessResult = await accessService.AddAccess(request.UserId, request.DocumentId);
            if (addAccessResult.IsFailure)
            {
                return BadRequest(addAccessResult.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [RoleAuthorize("user")]
    [HttpPost("Remove")]
    public async Task<IActionResult> Remove([FromQuery] ManageAccessRequest request)
    {
        try
        {
            if (!await CheckClientAccess(request.DocumentId))
            {
                return Forbid();
            }
            
            var deleteAccessResult = await accessService.DeleteAccess(request.UserId, request.DocumentId);
            if (deleteAccessResult.IsFailure)
            {
                return BadRequest(deleteAccessResult.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}