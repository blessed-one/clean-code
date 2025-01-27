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
            : Created();
    }
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpGet("Get/{documentId:guid}")]
    public async Task<IActionResult> GetDocById([FromRoute] Guid documentId)
    {
        var docByIdResult = await documentService.GetContentById(documentId);
        return docByIdResult.IsFailure
            ? Problem(docByIdResult.Message)
            : File(docByIdResult.Data!, "text/plain");
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
}