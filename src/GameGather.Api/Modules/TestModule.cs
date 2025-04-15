using GameGather.Application.Utils.Email;
using Microsoft.AspNetCore.Mvc;
using static GameGather.Api.Common.HttpResultsExtensions;

namespace GameGather.Api.Modules;

public static class TestModule
{
    public static void AddTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ping", async () =>
        {
            return Ok("pong");
        }).RequireAuthorization();

        app.MapPost("/api/test-email", async (
            [FromBody] EmailMessage emailMessage,
            [FromServices] IEmailService emailService
        ) =>
        {
            var response = await emailService.SendEmailAsync(emailMessage);
            return Ok(response);
        });
    }
}