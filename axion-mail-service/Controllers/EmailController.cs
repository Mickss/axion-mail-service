using Microsoft.AspNetCore.Mvc;
using axion_mail_service.Services;

namespace axion_mail_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        private static readonly List<Dictionary<string, (string Subject, string HtmlContent)>> AllTemplates = new()
        {
            EmailTemplatesWelcome.Templates,
        };

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail) ||
                string.IsNullOrWhiteSpace(request.EventType))
            {
                return BadRequest(new { error = "ToEmail and EventType are required" });
            }

            var template = AllTemplates
                .SelectMany(t => t)
                .FirstOrDefault(t => t.Key == request.EventType);

            if (template.Key == null)
            {
                return BadRequest(new { error = $"Unknown EventType: {request.EventType}" });
            }

            var result = await _emailService.SendEmailAsync(
                request.ToEmail,
                template.Value.Subject,
                template.Value.HtmlContent,
                request.ToName
            );

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new
                {
                    error = "Failed to send email",
                    details = result.ResponseText
                });
            }

            return Ok(new
            {
                message = "Email sent successfully",
                messageId = result.MessageId
            });
        }
    }

    public record EmailRequest(
        string ToEmail,
        string EventType,
        string? ToName = null
    );
}
