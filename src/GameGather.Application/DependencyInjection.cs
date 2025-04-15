using System.Reflection;
using FluentValidation;
using GameGather.Application.Common.Behavior;
using GameGather.Application.Configurations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GameGather.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.ConfigureOptions<PasswordOptionsSetup>();
        
        return services;
    }
}