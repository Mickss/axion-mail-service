using Microsoft.AspNetCore.Mvc;
using axion_mail_service.Services;

namespace axion_mail_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        private static readonly Dictionary<string, (string Subject, string HtmlContent)> Templates = new()
        {
            ["WELCOME"] = (
                Subject: "Witaj w app.disc-golf.pl! 👋",
                HtmlContent: """
                    <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                        <h2>Cześć! 🙌</h2>
                        <p>Fajnie, że jesteś z nami! Twoje konto w <strong>app.disc-golf.pl</strong> zostało właśnie utworzone.</p>

                        <h3>Co tu znajdziesz?</h3>
                        <ul>
                            <li>📅 aktualne turnieje i wydarzenia disc golfowe w Polsce</li>
                            <li>🔔 przypomnienia o zbliżającej się rejestracji na turnieje</li>
                            <li>📍 jedno miejsce zamiast przeszukiwania Facebooka i dziesięciu grup naraz</li>
                        </ul>

                        <p>To narzędzie robione przez graczy dla graczy. Ma być prosto, czytelnie i bez zbędnego chaosu.</p>

                        <p>👉 Wejdź, sprawdź co się dzieje i dodaj kolejne zawody do kalendarza.<br/>
                        Jeśli czegoś brakuje albo coś nie działa – daj znać.<br/>
                        Ta aplikacja będzie cały czas się rozwijać.</p>

                        <br/>
                        <p>Pozdrawiamy,<br/><strong>Zespół app.disc-golf.pl</strong></p>
                    </div>
                """
            )
            // ["TOURNAMENT_REMINDER"] = ( Subject: "...", HtmlContent: "..." )
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

            if (!Templates.TryGetValue(request.EventType, out var template))
            {
                return BadRequest(new { error = $"Unknown EventType: {request.EventType}" });
            }

            var result = await _emailService.SendEmailAsync(
                request.ToEmail,
                template.Subject,
                template.HtmlContent,
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
