using API.Filters;
using API.Requests;
using Application.Interfaces.Services;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController(
    IDocumentService documentService) : ControllerBase
{
    [RoleAuthorize("admin")]
    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var allDocsResult = await documentService.GetAll();

        return allDocsResult.IsFailure 
            ? Problem(allDocsResult.Message) 
            : Ok(allDocsResult.Data);
    }
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [HttpGet("My")]
    public async Task<IActionResult> GetUsersDocs()
    {
        var docsByAuthorResult = await documentService.GetByAuthorId((Guid)HttpContext.Items["UserId"]!);
        return docsByAuthorResult.IsFailure 
            ? Problem(docsByAuthorResult.Message) 
            : Ok(docsByAuthorResult.Data);
    }
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [HttpGet("Shared")]
    public async Task<IActionResult> GetUsersSharedDocs()
    {
        var docsByAuthorAccessResult = await documentService.GetByAuthorIdAccess((Guid)HttpContext.Items["UserId"]!);
        return docsByAuthorAccessResult.IsFailure 
            ? Problem(docsByAuthorAccessResult.Message) 
            : Ok(docsByAuthorAccessResult.Data);
    }
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [HttpPost("New")]
    public async Task<IActionResult> Create([FromBody] DocumentNameRequest request)
    {
        var documentName = request.Name;
        if (string.IsNullOrEmpty(documentName))
        {
            return BadRequest("documentName is required");
        }

        var createDocResult = await documentService.Create(documentName, (Guid)HttpContext.Items["UserId"]!);
        return createDocResult.IsFailure 
            ? Problem(createDocResult.Message) 
            : Ok(createDocResult.Data);
    }
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpGet("Get/{documentId:guid}")]
    public async Task<IActionResult> GetDocById([FromRoute] Guid documentId)
    {
        var docContentByIdResult = await documentService.GetContentById(documentId);
        if (docContentByIdResult.IsFailure)
        {
            return Problem(docContentByIdResult.Message);
        }
        
        var docByIdResult = await documentService.GetById(documentId);
        if (docByIdResult.IsFailure)
        {
            return Problem(docByIdResult.Message);
        }

        
        return File(docContentByIdResult.Data!, "text/plain", $"{docByIdResult.Data!.Name}.md");

    }
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateDoc([FromBody] DocumentUpdateRequest request)
    {
        var docUpdateResult = await documentService.Update(request.DocumentId, request.Text);
        return docUpdateResult.IsFailure
            ? Problem(docUpdateResult.Message)
            : Ok();
    }

    [RoleAuthorize("user")]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] DocumentIdRequest request)
    {
        var deleteDocResult = await documentService.Delete(request.DocumentId);
        
        return deleteDocResult.IsFailure 
            ? Problem(deleteDocResult.Message) 
            : Ok();
    }

    [RoleAuthorize("user")]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpPatch("Rename")]
    public async Task<IActionResult> Rename([FromBody] DocumentUpdateRequest request)
    {
        var renameRequest = await documentService.Rename(request.DocumentId, request.Text);

        return renameRequest.IsFailure
            ? Problem(renameRequest.Message)
            : Ok();
    }
}