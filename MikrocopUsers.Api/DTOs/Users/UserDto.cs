namespace MikrocopUsers.Api.DTOs.Users;

public record UserDto
{
    public required string Id { get; init; }
    public required string UserName { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string MobileNumber { get; init; }
    public required string Language { get; init; }
    public required string Culture { get; init; }
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
    public string? Company { get; init; }
    public required string Password { get; init; }
}
