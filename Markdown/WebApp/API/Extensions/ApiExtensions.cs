using MarkdownRealisation.Classes;
using MarkdownRealisation.Interfaces;

namespace API.Extensions;

public static class ApiExtensions
{
    public static void AddMdProcessor(this IServiceCollection services)
    {
        services.AddSingleton<IParser, Parser>();
        services.AddSingleton<ITagsResolver, Resolver>();
        services.AddSingleton<IRender, Md>();
    }
}