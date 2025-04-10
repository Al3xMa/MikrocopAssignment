using MikrocopUsers.Api.Entities;

namespace MikrocopUsers.Api.DTOs.Users;

public static class UserMappings
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            MobileNumber = user.MobileNumber,
            Language = user.Language,
            Culture = user.Culture,
            Password = user.Password
        };
    }
    public static User ToEntity(this CreateUserDto createUserDto)
    {
        return new User
        {
            Id = User.NewId(),
            UserName = createUserDto.UserName,
            FullName = createUserDto.FullName,
            Email = createUserDto.Email,
            MobileNumber = createUserDto.MobileNumber,
            Language = createUserDto.Language,
            Culture = createUserDto.Culture,
            Password = createUserDto.Password
        };
    }
    public static void UpdateFromDto(this User user, UpdateUserDto updateUserDto)
    {
       user.JobTitle = updateUserDto.JobTitle;
       user.Department = updateUserDto.Department;
       user.Company = updateUserDto.Company;
    }
}
