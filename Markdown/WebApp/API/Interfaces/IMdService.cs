using Application;

namespace API.Interfaces;

public interface IMdService
{
    Task<Result<string>> RenderHtml(string markdown);
}