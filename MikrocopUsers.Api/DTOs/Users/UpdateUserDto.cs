namespace MikrocopUsers.Api.DTOs.Users;

public record UpdateUserDto
{
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
    public string? Company { get; init; }
}
