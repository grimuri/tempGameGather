using GameGather.Api.Modules;
using GameGather.Application;
using GameGather.Infrastructure;
using GameGather.Infrastructure.Authentication;
using GameGather.Infrastructure.Utils.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Wprowadź token JWT w polu 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
    
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

    
var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        
    }

    app.UseSwagger(); 
    app.UseSwaggerUI();

    app.AddTestEndpoints();
    app.AddAuthenticationEndpoints();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();
    
    app.ApplyMigration();

    app.Run();
}

