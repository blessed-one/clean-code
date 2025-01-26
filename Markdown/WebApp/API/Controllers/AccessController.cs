using System.Security.Claims;
using API.Filters;
using API.Requests;
using Application.Interfaces.Services;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessController(IDocumentAccessService accessService) : ControllerBase
{
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(AccessFilter))]
    [HttpGet("Give")]
    public async Task<IActionResult> Give([FromQuery] ManageAccessRequest request)
    {
        try
        {
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
    
    [RoleAuthorize("user")]
    [ServiceFilter(typeof(AccessFilter))]
    [HttpPost("Remove")]
    public async Task<IActionResult> Remove([FromQuery] ManageAccessRequest request)
    {
        try
        {
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