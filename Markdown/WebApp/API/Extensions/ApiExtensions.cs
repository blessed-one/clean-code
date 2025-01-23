using Microsoft.EntityFrameworkCore;
using MarkdownRealisation.Classes;
using MarkdownRealisation.Interfaces;
using Persistence;
using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static class ApiExtensions
{
    public static void AddMdProcessor(this IServiceCollection services)
    {
        services.AddSingleton<IParser, Parser>();
        services.AddSingleton<ITagsResolver, Resolver>();
        services.AddSingleton<IRender, Md>();
        services.AddSingleton<IMdService, MdService>();
    }

    public static void AddPostgresDb(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
            });
    }
}