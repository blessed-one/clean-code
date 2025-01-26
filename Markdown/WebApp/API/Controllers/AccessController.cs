using API.Filters;
using API.Requests;
using Application.Interfaces.Services;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessController(IDocumentAccessService accessService) : ControllerBase
{
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpPost("Give")]
    public async Task<IActionResult> Give([FromBody] ManageAccessRequest request)
    {
        var addAccessResult = await accessService.AddAccess(request.UserId, request.DocumentId);
        if (addAccessResult.IsFailure)
        {
            return BadRequest(addAccessResult.Message);
        }

        return Ok();
    }

    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [ServiceFilter(typeof(HasAccessFilter))]
    [HttpDelete("Remove")]
    public async Task<IActionResult> Remove([FromBody] ManageAccessRequest request)
    {
        var deleteAccessResult = await accessService.DeleteAccess(request.UserId, request.DocumentId);
        if (deleteAccessResult.IsFailure)
        {
            return BadRequest(deleteAccessResult.Message);
        }

        return Ok();
    }
}