using Microsoft.AspNetCore.Mvc;
using axion_mail_service.Services;

namespace axion_mail_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.HtmlContent))
            {
                return BadRequest(new { error = "ToEmail, Subject and HtmlContent are required" });
            }

            var result = await _emailService.SendEmailAsync(
                request.ToEmail,
                request.Subject,
                request.HtmlContent,
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
        string Subject,
        string HtmlContent,
        string? ToName = null
    );
}
