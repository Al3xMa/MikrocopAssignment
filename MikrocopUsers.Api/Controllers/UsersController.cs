using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MikrocopUsers.Api.DTOs;
using MikrocopUsers.Api.DTOs.Users;
using MikrocopUsers.Api.Entities;
using MikrocopUsers.Api.Filters;
using MikrocopUsers.Api.Repositories.Users;
using MikrocopUsers.Api.Services;

namespace MikrocopUsers.Api.Controllers;

[ApiKey]
[ApiController]
[Route("users")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns> All users</returns>
    [HttpGet]
    [ProducesResponseType<CollectionResponse<UserDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CollectionResponse<UserDto>>> GetUsers()
    {
        IEnumerable<User> users = await userRepository.GetAllAsync();
        if (!users.Any())
        {
            return NotFound();
        }

        IEnumerable<UserDto> userDtos = users.Select(u => u.ToDto());

        var response = new CollectionResponse<UserDto>
        {
            Data = userDtos.ToList()
        };
        return Ok(response);
    }

    /// <summary>
    /// Get user by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The requested user</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(string id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="createUserDto"></param>
    /// <param name="validator"></param>
    /// <param name="passwordHasher"></param>
    /// <returns>Created user</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateUser(CreateUserDto createUserDto, IValidator<CreateUserDto> validator, IPasswordHasher passwordHasher)
    {
        await validator.ValidateAndThrowAsync(createUserDto);

        User? existingUser = await userRepository.GetByUsernameAsync(createUserDto.UserName);
        if (existingUser != null)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "User with that username already exists.");
        }

        string passwordHash = passwordHasher.HashPassword(createUserDto.Password);

        createUserDto.Password = passwordHash;
        User? user = createUserDto.ToEntity();

        await userRepository.CreateAsync(user);

        UserDto? userDto = user.ToDto();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDto);
    }

    /// <summary>
    /// Update user by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateUserDto"></param>
    /// <returns>No content on success</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateUserDto)
    {
        User? existingUser = await userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        await userRepository.UpdateAsync(id, updateUserDto);
        return NoContent();
    }

    /// <summary>
    /// Delete user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        User? existingUser = await userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        await userRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="changePasswordDto"></param>
    /// <param name="passwordHasher"></param>
    /// <returns>No content on success</returns>
    [HttpPost("{id}/change-password")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto changePasswordDto, IPasswordHasher passwordHasher)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        if (!passwordHasher.VerifyPassword(changePasswordDto.OldPassword, user.Password))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Old password is incorrect.");
        }

        changePasswordDto.NewPassword = passwordHasher.HashPassword(changePasswordDto.NewPassword);
        await userRepository.UpdatePassword(user.Id, changePasswordDto);
        return NoContent();
    }
}
