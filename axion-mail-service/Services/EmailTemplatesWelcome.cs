namespace axion_mail_service.Services
{
    public static class EmailTemplatesWelcome
    {
        public static readonly Dictionary<string, (string Subject, string HtmlContent)> Templates = new()
        {
            ["WELCOME"] = (
                Subject: "Witaj w app.disc-golf.pl! 👋",
                HtmlContent: """
                    <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                        <h2>Cześć 🙌</h2>
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
    }
}
