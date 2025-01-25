using Microsoft.AspNetCore.Mvc;
using API.Requests;


namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PageController() : ControllerBase
{
    [HttpGet("")]
    public IActionResult Index()
    {
        var htmlContent = "<html><body><h1>hello md processor</h1></body></html>";

        return Content(htmlContent, "text/html");
    }

    [HttpGet("Convertor")]
    public IActionResult ShowConvertorPage()
    {
        return File("ConvertorPage.html", "text/html");
    }
}