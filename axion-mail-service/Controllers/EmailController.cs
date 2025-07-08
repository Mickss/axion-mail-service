using Microsoft.AspNetCore.Mvc;

namespace axion_mail_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost("send-reset-link")]
        public IActionResult SendResetLink([FromBody] EmailRequest request)
        {
            Console.WriteLine($"[C# EmailService] Password reset request received for: {request.Email}");
            return Ok(new { message = "Reset link sent (simulated)." });
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; }
    }
}
