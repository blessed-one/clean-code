using API.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController(IDocumentService documentService) : ControllerBase
{
    [Authorize]
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
    [HttpPost("New")]
    public async Task<IActionResult> Create([FromQuery] string documentName)
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(documentName))
        {
            return BadRequest("documentName is required");
        }

        var createDocRequest = await documentService.Create(documentName, userId);

        if (createDocRequest.IsFailure)
        {
            return Problem(createDocRequest.Message);
        }
        
        return Created();
    }
}