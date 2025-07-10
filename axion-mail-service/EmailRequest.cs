using System.Text.Json.Serialization;

namespace axion_mail_service
{
    public class EmailRequest
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string TemplateId { get; set; }
        public List<RecipientVariable> Variables { get; set; }
        public string Subject { get; set; }
    }

    public class Substitution
    {
        public string Var { get; set; }
        public string Value { get; set; }
    }

    public class RecipientVariable
    {
        public string Email { get; set; }
        public Dictionary<string, Substitution> Substitutions { get; set; }
    }
}
