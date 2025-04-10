using FluentValidation;

namespace MikrocopUsers.Api.DTOs.Users;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .Length(3, 50)
            .WithMessage("Username must be between 3 and 50 characters.");
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .Length(3, 100)
            .WithMessage("Full name must be between 3 and 100 characters.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(100)
            .WithMessage("Max allowed characters is 100.")
            .EmailAddress()
            .WithMessage("Invalid Email format.");
        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .WithMessage("Mobile number is required.")
            .Matches(@"^\+?[0-9]{9,15}$")
            .WithMessage("Invalid Mobile number format.");
        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage("Language is required.")
            .Length(2, 10)
            .WithMessage("Language must be between 3 and 10 characters.");
        RuleFor(x => x.Culture)
            .NotEmpty()
            .WithMessage("Culture is required.")
            .MaximumLength(10)
            .WithMessage("Max allowed characters is 10.");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(16)
            .WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+")
            .WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("Your password must contain at least one number.");
    }
}
