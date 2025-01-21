using API.Extensions;
using MarkdownRealisation.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.AddMdProcessor();

var app = builder.Build();

app.Run(async context =>
{
    var mdService = app.Services.GetService<IRender>();

    var md = "_aaa_";
    var html = mdService.RenderHtml(md);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(html);
});


app.Run();