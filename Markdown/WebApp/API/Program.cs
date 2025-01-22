using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
builder.Logging.AddFile("logs.txt");

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


services.AddMdProcessor();
services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseStaticFiles();

app.Run(async (context) =>
{
    app.Logger.LogInformation($"Path: {context.Request.Path}, Method: {context.Request.Method}");
    await context.Response.WriteAsync("Hello World!");
});

app.Run();