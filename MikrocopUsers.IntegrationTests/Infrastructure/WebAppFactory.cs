using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace MikrocopUsers.IntegrationTests.Infrastructure;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _sqlContainer.GetConnectionString());
    }
    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
       await _sqlContainer.DisposeAsync();
    }
}
