namespace MikrocopUsers.Api.DTOs.Users;

public record ChangePasswordDto
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; set; }
}
