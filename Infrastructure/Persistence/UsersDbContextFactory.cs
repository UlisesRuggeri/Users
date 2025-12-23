

using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersContext> 
{
    public UsersContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        var builder = new DbContextOptionsBuilder<UsersContext>();
        var connectionString = configuration.GetConnectionString("Connection");

        builder.UseSqlServer(connectionString);

        return new UsersContext(builder.Options);
    }
}
