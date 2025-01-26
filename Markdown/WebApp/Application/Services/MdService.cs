using Application.Interfaces.Services;
using Application.Utils;
using MarkdownRealisation.Interfaces;

namespace Application.Services;

public class MdService : IMdService
{
    private readonly IRender _markdownRender;

    public MdService(IRender markdownRenderer)
    {
        _markdownRender = markdownRenderer;        
    }

    public async Task<Result<string>> RenderHtml(string markdown)
    {
        try
        {
            return Result<string>.Success(_markdownRender.RenderHtml(markdown));
        }
        catch
        {
            return Result<string>.Failure("Failed to render markdown");
        }
    }
}