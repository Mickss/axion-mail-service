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
            var result = await _emailService.SendEmailAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { error = result.ResponseText });
            }

            return Ok(new { message = "Email sent successfully", mailerSendResponse = result.ResponseText });
        }
    }
}