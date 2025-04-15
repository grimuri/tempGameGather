using GameGather.Application.Utils.Email;
using GameGather.Domain.DomainEvents;
using MediatR;

namespace GameGather.Application.Features.Users.Events;

public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegistered>
{
    private readonly IEmailService _emailService;

    public UserRegisteredDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var message = new EmailMessage(
            "Verify your email",
            "Welcome to GameGather",
            $"""
                <h1>Welcome to GameGather</h1>
                <p>Hi {notification.FirstName},</p>
                <p>Thank you for registering on GameGather. 
                Please verify your email address by pass this code: 
                {notification.VerificationToken}</p>
                """,
            notification.Email);
        
        await _emailService.SendEmailAsync(message);
    }
}