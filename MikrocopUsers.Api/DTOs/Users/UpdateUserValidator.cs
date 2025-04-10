using FluentValidation;

namespace MikrocopUsers.Api.DTOs.Users;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.JobTitle)
            .NotEmpty()
            .WithMessage("Job title is required.")
            .Length(3, 100)
            .WithMessage("Job title must be between 3 and 100 characters.");
        RuleFor(x => x.Department)
            .NotEmpty()
            .WithMessage("Department is required.")
            .Length(3, 100)
            .WithMessage("Department must be between 3 and 100 characters.");
        RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company is required.")
            .Length(3, 100)
            .WithMessage("Company must be between 3 and 100 characters.");
    }
}
