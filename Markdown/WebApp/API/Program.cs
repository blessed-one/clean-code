using API.Extensions;
using API.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// logging
builder.Logging.AddFile("logs.txt");

// settings
builder.Configuration.AddEnvironmentVariables();

// db
services.AddPostgresDb(configuration);
services.AddRepositories();
services.AddMinioS3(configuration);

// swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// services
services.AddScoped<IUserService, UserService>();
services.AddScoped<IDocumentService, DocumentService>();
services.AddScoped<IDocumentAccessService, DocumentAccessService>();

// md
services.AddMdProcessor();

// controllers, filters
services.AddControllers();
services.AddScoped<HasAccessFilter>();
services.AddScoped<UserExistFilter>();

// reg-auth
services.AddAuthenticator(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.MapControllers();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.MapGet("/", () => $"connection string: {configuration.GetConnectionString("AppDbContext")}");
app.Run();