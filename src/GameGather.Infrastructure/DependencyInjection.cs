using System.Reflection.Metadata;
using GameGather.Application.Persistance;
using GameGather.Application.Utils;
using GameGather.Application.Utils.Email;
using GameGather.Infrastructure.Authentication;
using GameGather.Infrastructure.BackgroundJobs;
using GameGather.Infrastructure.Persistance;
using GameGather.Infrastructure.Repositories;
using GameGather.Infrastructure.Utils;
using GameGather.Infrastructure.Utils.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Quartz;

namespace GameGather.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<GameGatherDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                r =>
                    r.MigrationsAssembly(typeof(DependencyInjection).Assembly.ToString())));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<EmailOptionsSetup>();

        services.AddAuthorization();

        services.AddHttpContextAccessor();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithIdentity("ProcessOutboxMessagesJob-trigger")
                        .WithSimpleSchedule(schedule =>
                            schedule
                                .WithIntervalInSeconds(10)
                                .RepeatForever())
                );
            
        });

        services.AddQuartzHostedService();

        
        return services;
    }
}