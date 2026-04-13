namespace axion_mail_service.Services
{
    public class EmailTemplatesPasswordReset
    {
        public static readonly Dictionary<string, (string Subject, string HtmlContent)> Templates = new()
        {
            ["PASSWORD_RESET"] = (
                Subject: "Zmiana hasła – app.disc-golf.pl",
                HtmlContent: """
                    <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                        <h2>Cześć! 👋</h2>
                        <p>Otrzymaliśmy prośbę o zmianę hasła do Twojego konta w <strong>app.disc-golf.pl</strong>.</p>
                        <p>Aby ustawić nowe hasło, kliknij w link poniżej:</p>
                        <p>
                            <a href="{0}" 
                               style="background-color: #1976d2; color: white; padding: 12px 24px; 
                                      text-decoration: none; border-radius: 4px; display: inline-block;">
                                Resetuj hasło
                            </a>
                        </p>
                        <p>Jeśli to nie Ty inicjowałeś zmianę hasła – zignoruj tę wiadomość.<br/>
                        Link może zostać użyty tylko raz.</p>
                        <br/>
                        <p>Pozdrawiamy,<br/><strong>Zespół app.disc-golf.pl</strong></p>
                    </div>
                """
            )
        };
    }
}
