using FluentValidation;
using MikrocopUsers.Api.Middleware;
using MikrocopUsers.Api.Repositories.Users;
using MikrocopUsers.Api.Services;
using MikrocopUsers.Api.Settings;
using Serilog;
using Serilog.Events;

namespace MikrocopUsers.Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Api Key to access the API",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "x-api-key",
                Scheme = "ApiKeyScheme"
            });
            x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                { new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        return builder;
    }
    public static WebApplicationBuilder AddErrorHandling(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return builder;
    }

    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        return builder;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File("/var/logs/log-.txt", rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "MikrocopUsers.Api")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithMachineName()
            .Enrich.WithClientIp()
            .Enrich.WithRequestHeader("x-api-key")
            .CreateLogger();

        builder.Services.AddSerilog();
        return builder;
    }
}
