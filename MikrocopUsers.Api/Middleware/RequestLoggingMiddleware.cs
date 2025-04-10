using System.Text;
using System.Text.Json;
using Serilog;

namespace MikrocopUsers.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };
    private readonly List<string> _sensitiveFields;

    public RequestLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _sensitiveFields = configuration.GetSection("SensitiveFields").Get<List<string>>() ?? [];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Enable buffering to allow reading the request body multiple times
        context.Request.EnableBuffering();

        // Read the request body as a string
        string requestBody;
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);

        requestBody = await reader.ReadToEndAsync();


        // Reset the request body stream position so it can be read by the next middleware
        context.Request.Body.Position = 0;

        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var requestPath = context.Request.Path;
        var method = context.Request.Method;

        var sanitizedRequestBody = MaskPasswordInRequestBody(requestBody);

        Log.Information("Request: {Method} {Path} | Client IP: {ClientIp} | User Agent: {UserAgent} | {RequestBody}",
            method, requestPath, clientIp, userAgent, sanitizedRequestBody);

        await _next(context);
    }

    private string MaskPasswordInRequestBody(string requestBody)
    {
        try
        {
            // Parse the request body as JSON
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);

            if (jsonObject != null)
            {
                foreach (var field in _sensitiveFields)
                {
                    if (jsonObject.ContainsKey(field))
                    {
                        jsonObject[field] = "???";
                    }
                }
            }

            return JsonSerializer.Serialize(jsonObject, JsonOptions);
        }
        catch (JsonException)
        {
            // If parsing fails, return the original request body
            return requestBody;
        }
    }
}
