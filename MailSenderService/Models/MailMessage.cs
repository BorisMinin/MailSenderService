namespace MailSenderService.Models
{
    /// <summary>
    /// Represents a mail message to be sent by the mail sender service.
    /// </summary>
    public class MailMessage
    {
        // Recipient's email address.
        public string ToEmail { get; set; }
        // The subject of the email message.
        public string Subject { get; set; }
        // The body content of the email message.
        public string Body { get; set; }
        // The list of file attachments included with the email message.
        public List<IFormFile>? Attachments { get; set; }
    }
}