using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain;

public class AppContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder
            .UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings_App"), contextOptionsBuilder => contextOptionsBuilder.CommandTimeout(6000));

        return new ApplicationContext(optionsBuilder.Options);
    }
}