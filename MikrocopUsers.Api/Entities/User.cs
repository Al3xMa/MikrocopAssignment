using System.Reflection;

namespace MikrocopUsers.Api.Entities;

public class User
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string Language { get; set; }
    public string Culture { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? Company { get; set; }
    public string Password { get; set; }

    public static string NewId()
    {
        return $"u_{Guid.CreateVersion7()}";
    }
}


