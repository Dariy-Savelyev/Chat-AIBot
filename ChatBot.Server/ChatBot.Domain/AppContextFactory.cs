using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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