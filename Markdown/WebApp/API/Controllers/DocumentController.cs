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
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [HttpPost("New")]
    public async Task<IActionResult> Create([FromBody] DocumentRequest request)
    {
        var documentName = request.Name;
        if (string.IsNullOrEmpty(documentName))
        {
            return BadRequest("documentName is required");
        }

        var createDocResult = await documentService.Create(documentName, (Guid)HttpContext.Items["UserId"]!);
        if (createDocResult.IsFailure)
        {
            return Problem(createDocResult.Message);
        }

        return Created();
    }

    [RoleAuthorize("user")]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] DocumentRequest request)
    {
        var documentId = request.DocumentId;

        var deleteDocResult = await documentService.Delete(documentId);
        if (deleteDocResult.IsFailure)
        {
            return Problem(deleteDocResult.Message);
        }

        return Ok();
    }
}