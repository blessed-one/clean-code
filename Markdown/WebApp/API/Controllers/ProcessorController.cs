using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Requests;


namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessorController(IMdService mdService) : ControllerBase
{
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var htmlContent = "<html><body><h1>hello md processor</h1></body></html>";

        return Ok( new { Html = htmlContent });
    }

    [HttpPost("Convert")]
    public async Task<IActionResult> Convert([FromBody] MdRequest request)
    {
        var htmlRenderResult = await mdService.RenderHtml(request.MdText!);

        if (!htmlRenderResult.IsSuccess)
        {
            return BadRequest(htmlRenderResult.Message);
        }
        
        return Ok(new { Html = htmlRenderResult.Data });
    }
}