using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using MikrocopUsers.Api.DTOs.Users;

namespace MikrocopUsers.UnitTest.Validators;

public class CreateUserDtoValidatorTests
{
    private readonly CreateUserValidator _validator;

    public CreateUserDtoValidatorTests()
    {
        _validator = new CreateUserValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Is_Empty()
    {
        // Arrange
        var model = new CreateUserDto
        {
            UserName = "",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Password = "Password123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var model = new CreateUserDto
        {
            UserName = "johndoe",
            FullName = "John Doe",
            Email = "invalid-email",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Password = "Password123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        // Arrange
        var model = new CreateUserDto
        {
            UserName = "johndoe",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Password = "12345"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Dto_Is_Valid()
    {
        // Arrange
        var model = new CreateUserDto
        {
            UserName = "johndoe",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            MobileNumber = "+123456789",
            Language = "en",
            Culture = "en-US",
            Company = "Mikrocop",
            JobTitle = "Developer",
            Department = "IT",
            Password = "Password123"
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

