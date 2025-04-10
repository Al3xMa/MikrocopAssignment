
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MikrocopUsers.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!IsValidApiKey(context.HttpContext))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private bool IsValidApiKey(HttpContext context)
    {
        string? apiKey = context.Request.Headers[ApiKeyHeaderName];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return false;
        }

        string actualApiKey = context.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("ApiKey")!;

        return actualApiKey.Equals(apiKey);
    }
}
