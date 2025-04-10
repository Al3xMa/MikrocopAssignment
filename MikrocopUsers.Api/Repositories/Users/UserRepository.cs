
using Dapper;
using MikrocopUsers.Api.DTOs.Users;
using MikrocopUsers.Api.Entities;

namespace MikrocopUsers.Api.Repositories.Users;

public class UserRepository(IConfiguration configuration) : RepositoryBase(configuration), IUserRepository
{
    public async Task CreateAsync(User user)
    {
        const string insert = @"insert into Users (Id, UserName, FullName, Email, MobileNumber, Language, Culture, JobTitle, Department, Company, Password)
                                values (@Id, @UserName, @FullName, @Email, @MobileNumber, @Language, @Culture, @JobTitle, @Department, @Company, @Password)";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(insert, new
        {
            user.Id,
            user.UserName,
            user.FullName,
            user.Email,
            user.MobileNumber,
            user.Language,
            user.Culture,
            user.JobTitle,
            user.Department,
            user.Company,
            user.Password
        });
    }

    public async Task DeleteAsync(string id)
    {
        const string delete = @"delete from Users
                                where Id = @id";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(delete, new { id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string select = @"select Id, UserName, FullName, Email, MobileNumber, Language, Culture, JobTitle, Department, Company, Password
                                from Users";

        using var connection = CreateConnection();
        return await connection.QueryAsync<User>(select);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        const string select = @"select Id, UserName, FullName, Email, MobileNumber, Language, Culture, JobTitle, Department, Company, Password
                                from Users
                                where Id = @id";

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(select, new { id });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string select = @"select Id, UserName, FullName, Email, MobileNumber, Language, Culture, Password
                                from Users
                                where UserName = @username";

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(select, new { username });
    }

    public async Task UpdateAsync(string id, UpdateUserDto user)
    {
        const string update = @"update Users
                                set JobTitle = @JobTitle,
                                    Department = @Department,
                                    Company = @Company
                                where Id = @Id";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(update, new
        {
            user.JobTitle,
            user.Department,
            user.Company,
            id
        });
    }

    public async Task UpdatePassword(string id, ChangePasswordDto changePasswordDto)
    {
        const string update = @"update Users
                                set Password = @Password
                                where Id = @Id";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(update, new
        {
            Password = changePasswordDto.NewPassword,
            id
        });
    }
}
