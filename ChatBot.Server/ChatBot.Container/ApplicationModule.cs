﻿using ChatBot.Application.MapperProfiles;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Container;

public static class ApplicationModule
{
    public static WebApplicationBuilder LoadApplicationModule(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<IBaseService>()
            .AddClasses(classes => classes.AssignableTo(typeof(IBaseService)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        builder.Services.AddAutoMapper(typeof(TestProfile));

        builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();

        return builder;
    }
}