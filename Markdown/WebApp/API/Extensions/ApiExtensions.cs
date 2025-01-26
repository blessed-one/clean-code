using System.Text;
using Microsoft.EntityFrameworkCore;
using MarkdownRealisation.Classes;
using MarkdownRealisation.Interfaces;
using Persistence;
using Persistence.Repositories;
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

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

    public static void AddPostgresDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            options => { options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext))); });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UsersRepository>();
        services.AddScoped<IDocumentRepository, DocumentsRepository>();
        services.AddScoped<IDocumentAccessRepository, DocumentAccessRepository>();
    }

    public static void AddAuthenticator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddSingleton<IAuthorizationHandler, RoleHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, RolePolicyProvider>();

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];

                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RolePolicy", policy =>
            {
                policy.Requirements.Add(new RoleRequirement("admin", "user"));
            });
            
            options.AddPolicy("DynamicRolePolicy", policy =>
            {
                policy.Requirements.Add(new RoleRequirement(Array.Empty<string>()));
            });
        });
    }
}