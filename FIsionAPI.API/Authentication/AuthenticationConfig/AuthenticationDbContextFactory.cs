using FIsionAPI.API.Authentication.AuthenticationConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FIsionAPI.API.Authentication;

public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

        var connectionString = config.GetConnectionString("AuthenticationConnection");

        var options = new DbContextOptionsBuilder<AuthenticationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AuthenticationDbContext(options);
    }
}
