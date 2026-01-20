using System.Text.Json;

namespace axion_mail_service.Services
{
    public interface IEmailService
    {
        Task<EmailServiceResult> SendEmailAsync(string toEmail, string subject, string htmlContent, string? toName = null);
    }

    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(
            IHttpClientFactory httpClientFactory,
            ILogger<EmailService> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<EmailServiceResult> SendEmailAsync(
            string toEmail,
            string subject,
            string htmlContent,
            string? toName = null)
        {
            var client = _httpClientFactory.CreateClient("Brevo");
            var fromEmail = _configuration["Brevo:FromEmail"];
            var fromName = _configuration["Brevo:FromName"];

            var brevoPayload = new
            {
                sender = new
                {
                    name = fromName,
                    email = fromEmail
                },
                to = new[]
                {
                    new
                    {
                        email = toEmail,
                        name = toName ?? toEmail
                    }
                },
                subject = subject,
                htmlContent = htmlContent
            };

            try
            {
                var response = await client.PostAsJsonAsync("smtp/email", brevoPayload);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<JsonElement>(responseText);
                    var messageId = result.TryGetProperty("messageId", out var id)
                        ? id.ToString()
                        : "sent";

                    _logger.LogInformation($"Email sent to {toEmail}. MessageId: {messageId}");

                    return new EmailServiceResult
                    {
                        IsSuccess = true,
                        StatusCode = (int)response.StatusCode,
                        ResponseText = responseText,
                        MessageId = messageId
                    };
                }
                else
                {
                    _logger.LogError($"Brevo error ({response.StatusCode}): {responseText}");

                    return new EmailServiceResult
                    {
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        ResponseText = responseText
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception sending email: {ex.Message}");

                return new EmailServiceResult
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    ResponseText = $"Internal error: {ex.Message}"
                };
            }
        }
    }

    public class EmailServiceResult
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string ResponseText { get; set; } = string.Empty;
        public string? MessageId { get; set; }
    }
}
