using System.Security.Claims;
using Application.Interfaces.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController(IDocumentService documentService, IDocumentAccessService accessService) : ControllerBase
{
    [RoleAuthorize("admin")]
    [HttpGet("All")]
    public async Task<IActionResult> All()
    {
        var allDocsResult = await documentService.GetAll();

        if (allDocsResult.IsFailure)
        {
            return BadRequest(allDocsResult.Message);
        }

        return Ok(allDocsResult.Data);
    }

    [Authorize]
    [RoleAuthorize("admin", "user")]
    [HttpPost("New")]
    public async Task<IActionResult> Create([FromQuery] string documentName)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(documentName))
        {
            return BadRequest("documentName is required");
        }

        var createDocResult = await documentService.Create(documentName, userId);
        if (createDocResult.IsFailure)
        {
            return Problem(createDocResult.Message);
        }

        return Created();
    }

    [RoleAuthorize("admin", "user")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid documentId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var accessResult = await accessService.ValidateAccess(userId, documentId);
        if (accessResult.IsFailure)
        {
            return Problem(accessResult.Message);
        }

        var hasAccess = accessResult.Data;
        if (!hasAccess)
        {
            return Forbid();
        }

        var docServiceResult = await documentService.Delete(documentId);
        if (docServiceResult.IsFailure)
        {
            return Problem(docServiceResult.Message);
        }

        return Ok();
    }
}