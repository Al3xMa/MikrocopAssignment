using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using MikrocopUsers.Api.DTOs.Users;

namespace MikrocopUsers.UnitTest.Validators;
public class ChangePasswordValidatorTests
{
    private readonly ChangePasswordValidator _validator;

    public ChangePasswordValidatorTests()
    {
        _validator = new ChangePasswordValidator();
    }

    [Fact]
    public void Should_Have_Error_When_OldPassword_Is_Empty()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "",
            NewPassword = "Password123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OldPassword);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Is_Empty()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = ""
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Is_Too_Short()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = "Short1"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Is_Too_Long()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = "ThisPasswordIsWayTooLong123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Missing_Uppercase()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = "password123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Missing_Lowercase()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = "PASSWORD123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Missing_Number()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = "PasswordOnly"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        // Arrange
        var model = new ChangePasswordDto
        {
            OldPassword = "OldPassword123",
            NewPassword = "Password123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
