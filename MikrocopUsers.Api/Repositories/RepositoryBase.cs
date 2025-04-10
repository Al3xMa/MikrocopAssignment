using System.Data;
using Microsoft.Data.SqlClient;

namespace MikrocopUsers.Api.Repositories;

public class RepositoryBase
{
    private readonly string _connectionString;

    public RepositoryBase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
