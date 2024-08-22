using ChatBot.Container;
using ChatBot.CrossCutting.Apm.Configuration;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Web.Middlewares;
using FluentValidation.AspNetCore;

namespace ChatBot.Web
{
    public class Program
    {
        private const string PolicyName = "AllowSpecificOrigin";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();

            builder.LoadDomainModule();
            builder.LoadApplicationModule();

            builder.Services.AddEndpointsApiExplorer();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(
                        PolicyName,
                        corsPolicyBuilder =>
                        {
                            corsPolicyBuilder.WithOrigins("https://localhost:5174", "https://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                });
            }

            builder.Host.ConfigureLogging();
            //builder.Host.UseElasticApm();
            builder.Services.AddLogging(builder);

            var app = builder.Build();

            var logger = app.Services.GetService<ILogger<Program>>();
            logger?.LogInformation("START Application");

            app.Migrate();
            app.Services.SeedRoles();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(PolicyName);

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();

            logger?.LogInformation("STOP Application");
        }
    }
}