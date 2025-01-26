using System.Security.Claims;
using API.Filters;
using API.Requests;
using Application.Interfaces.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController(
    IDocumentService documentService, 
    IDocumentAccessService accessService) : ControllerBase
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
    [RoleAuthorize("user")]
    [HttpPost("New")]
    public async Task<IActionResult> Create([FromBody] DocumentRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var documentName = request.Name;
        
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

    [RoleAuthorize("user")]
    [ServiceFilter(typeof(AccessFilter))]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] DocumentRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }
        
        var documentId = request.DocumentId;

        var docServiceResult = await documentService.Delete(documentId);
        if (docServiceResult.IsFailure)
        {
            return Problem(docServiceResult.Message);
        }

        return Ok();
    }
}