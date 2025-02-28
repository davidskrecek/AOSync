using AOSync.DAL.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AOSync.DAL.Factory;

public class AOSyncDbContextFactory : IDesignTimeDbContextFactory<AOSyncDbContext>
{
    public AOSyncDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Use working directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var profile = configuration["ConfigurationProfile"] ?? "Alfa";
        
        var profileConfig = configuration.GetSection(profile);

        var optionsBuilder = new DbContextOptionsBuilder<AOSyncDbContext>();
        var connectionString = profileConfig["ConnectionString"];
        
        optionsBuilder.UseMySql(connectionString!, new MySqlServerVersion(new Version(8, 0, 21)));

        return new AOSyncDbContext(optionsBuilder.Options);
    }
}