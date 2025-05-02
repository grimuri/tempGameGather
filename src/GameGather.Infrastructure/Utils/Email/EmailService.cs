using GameGather.Application.Utils.Email;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace GameGather.Infrastructure.Utils.Email;

public sealed class EmailService : IEmailService
{
    private readonly MailjetClient _client;
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
        _client = new MailjetClient(
            _emailOptions.ApiKeyPublic,
            _emailOptions.ApiKeyPrivate
        );
    }

    public async Task<string> SendEmailAsync(EmailMessage emailMessage)
    {
        var request = new MailjetRequest
        {
            Resource = Send.Resource
        };
        
        request
            .Property(Send.FromEmail, _emailOptions.FromEmail)
            .Property(Send.FromName, _emailOptions.FromName)
            .Property(Send.Subject, emailMessage.Subject)
            .Property(Send.TextPart, emailMessage.TextPart)
            .Property(Send.HtmlPart, emailMessage.HtmlPart)
            .Property(Send.Recipients, new JArray
            {
                new JObject
                {
                    { "Email", emailMessage.ToEmail }
                }
            });
        
        var response = await _client.PostAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            return $"Total: {response.GetTotal()}, Count: {response.GetCount()}\n Message: {response.GetData()}";
        }
        else
        {
            return
                $"StatusCode: {response.StatusCode}\n Error: {response.GetErrorInfo()}\n ErrorMessage: {response.GetErrorMessage()}";
        }
    }
}