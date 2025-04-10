using MikrocopUsers.Api;
using MikrocopUsers.Api.Middleware;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .AddApiServices()
    .AddApplicationServices()
    .AddErrorHandling()
    .AddLogging();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();


public partial class Program;
