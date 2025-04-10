using MikrocopUsers.Api.DTOs.Users;
using MikrocopUsers.Api.Entities;

namespace MikrocopUsers.Api.Repositories.Users;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task CreateAsync(User user);
    Task UpdateAsync(string id, UpdateUserDto user);
    Task DeleteAsync(string id);
    Task UpdatePassword(string id, ChangePasswordDto changePasswordDto);
}
