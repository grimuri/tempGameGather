namespace GameGather.Application.Utils.Email;

public interface IEmailService
{
    Task<string> SendEmailAsync(EmailMessage emailMessage);
}