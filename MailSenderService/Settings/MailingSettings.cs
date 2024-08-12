namespace MailSenderService.Settings
{
    /// <summary>
    /// Represents the settings required to configure and send emails using an SMTP server.
    /// </summary>
    public class MailingSettings
    {
        // The Mail-Id from which you would want to send a mail from
        public string Mail { get; set; }
        // The Name that appears to the receiver
        public string DisplayName { get; set; }
        // Valid password to login to SMTP server
        public string Password { get; set; }
        // Server URL for SMTP
        public string Host { get; set; }
        // Port Number of the SMTP server
        public int Port { get; set; }
    }
}