using FluentValidation;

namespace MikrocopUsers.Api.DTOs.Users;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .WithMessage("Old password is required.");
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(8)
            .WithMessage("New password must be at least 8 characters long.")
            .MaximumLength(16)
            .WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]")
            .WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("New password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
            .WithMessage("New password must contain at least one number.");
    }
}
