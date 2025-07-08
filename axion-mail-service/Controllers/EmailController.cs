using Microsoft.AspNetCore.Mvc;

namespace axion_mail_service.Controllers
{
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("send-email")]
        public IActionResult SendResetLink([FromBody] EmailRequest request)
        {
            Console.WriteLine($"[C# EmailService] Request: {request.Email}, {request.TemplateId}");
            return Ok(new { message = "Email sent." });
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; }
        public string TemplateId { get; set; }
    }
}
