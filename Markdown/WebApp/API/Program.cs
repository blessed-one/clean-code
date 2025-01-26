using API.Extensions;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// logging
builder.Logging.AddFile("logs.txt");

// db
services.AddPostgresDb(configuration);
services.AddRepositories();

// swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// services
services.AddScoped<IUserService, UserService>();
services.AddScoped<IDocumentService, DocumentService>();
services.AddScoped<IDocumentAccessService, DocumentAccessService>();


// business logic
services.AddMdProcessor();

// controllers
services.AddControllers();

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

app.Run();