using ChatBot.Container;
using ChatBot.CrossCutting.Constants;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Web.Middlewares;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ChatBot.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();

            builder.LoadDomainModule();
            builder.LoadApplicationModule();

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}