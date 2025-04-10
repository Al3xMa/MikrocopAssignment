using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikrocopUsers.Api.Services;

namespace MikrocopUsers.UnitTest.Services;
public class PasswordHasherServiceTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherServiceTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_Should_Return_HashedPassword()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.Contains("-", hashedPassword); // Ensure the format includes the hash and salt
    }

    [Fact]
    public void VerifyPassword_Should_Return_True_For_Valid_Password()
    {
        // Arrange
        var password = "TestPassword123";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_Should_Return_False_For_Invalid_Password()
    {
        // Arrange
        var password = "TestPassword123";
        var wrongPassword = "WrongPassword123";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(wrongPassword, hashedPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_Should_Return_False_For_Invalid_HashedPassword_Format()
    {
        // Arrange
        var password = "TestPassword123";
        var invalidHashedPassword = "InvalidFormat";

        // Act & Assert
        Assert.Throws<FormatException>(() =>
        {
            _passwordHasher.VerifyPassword(password, invalidHashedPassword);
        });
    }

    [Fact]
    public void HashPassword_Should_Generate_Different_Hashes_For_Same_Password()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hashedPassword1 = _passwordHasher.HashPassword(password);
        var hashedPassword2 = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotEqual(hashedPassword1, hashedPassword2); // Ensure unique salts generate different hashes
    }
}
