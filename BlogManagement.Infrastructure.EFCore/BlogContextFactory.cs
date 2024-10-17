using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BlogManagement.Infrastructure.EFCore;

public class BlogContextFactory : IDesignTimeDbContextFactory<BlogContext>
{
    public BlogContext CreateDbContext(string[] args = null)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory() + "/../ServiceHost")
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // اضافه کردن appsettings.Development.json
            .Build();

        var connectionString = configuration.GetConnectionString("LampshadeDb");

        var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new BlogContext(optionsBuilder.Options);
    }
}