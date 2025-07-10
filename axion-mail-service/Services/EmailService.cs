using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace axion_mail_service.Services
{
    public interface IEmailService
    {
        Task<EmailServiceResult> SendEmailAsync(EmailRequest request);
    }

    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MailerSendSettings _settings;

        public EmailService(
            IHttpClientFactory httpClientFactory,
            IOptions<MailerSendSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        public async Task<EmailServiceResult> SendEmailAsync(EmailRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiToken);

            var payload = new
            {
                template_id = request.TemplateId,
                to = new[] {
                    new {
                        email = request.RecipientEmail,
                        name = request.RecipientName
                    }
                },
                variables = request.Variables,
                from = new
                {
                    email = _settings.FromEmail,
                    name = _settings.FromName
                },
                subject = request.Subject
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.mailersend.com/v1/email", content);
            var responseText = await response.Content.ReadAsStringAsync();

            return new EmailServiceResult
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                ResponseText = responseText
            };
        }
    }

    public class EmailServiceResult
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string ResponseText { get; set; }
    }
}