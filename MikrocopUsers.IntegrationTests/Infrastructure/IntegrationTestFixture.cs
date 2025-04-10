using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Docker.DotNet.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MikrocopUsers.IntegrationTests.Infrastructure;
public abstract class IntegrationTestFixture(WebAppFactory webAppFactory) :IClassFixture<WebAppFactory>
{
    public HttpClient CreateClient() => webAppFactory.CreateClient();
    protected async Task CleanupDatabaseAsync()
    {
        using IServiceScope scope = webAppFactory.Services.CreateScope();
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
        {
            throw new InvalidOperationException("Database connection string not found in configuration");
        }

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();


        const string createTable = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
        BEGIN
            CREATE TABLE Users (
                Id VARCHAR(100) PRIMARY KEY,
                UserName VARCHAR(50) NOT NULL,
                FullName VARCHAR(100) NOT NULL,
                Email VARCHAR(100) NOT NULL,
                MobileNumber VARCHAR(20) NOT NULL,
                Language VARCHAR(10) NOT NULL,
                Culture VARCHAR(10) NOT NULL,
                JobTitle VARCHAR(100),
                Department VARCHAR(100),
                Company VARCHAR(100),
                Password VARCHAR(100) NOT NULL
            )
        END
        ELSE
        BEGIN
            TRUNCATE TABLE Users
        END";

        await connection.ExecuteAsync(createTable);

    }
}
