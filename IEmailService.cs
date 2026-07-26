using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EduTrack.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _from;

        public EmailService()
        {
            _smtpClient = new SmtpClient();
            _from = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"] ?? "noreply@edutrack.com";
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string bodyHtml)
        {
            return await SendEmailAsync(to, subject, bodyHtml, _from);
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string bodyHtml, string from = null)
        {
            try
            {
                var msg = new MailMessage(from ?? _from, to, subject, bodyHtml)
                {
                    IsBodyHtml = true
                };
                await _smtpClient.SendMailAsync(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string bodyHtml, byte[] attachment, string attachmentName)
        {
            try
            {
                var msg = new MailMessage(_from, to, subject, bodyHtml) { IsBodyHtml = true };
                if (attachment != null && attachment.Length > 0)
                {
                    msg.Attachments.Add(new Attachment(new System.IO.MemoryStream(attachment), attachmentName));
                }
                await _smtpClient.SendMailAsync(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendVerificationEmailAsync(string email, string verificationLink, string fullName)
        {
            string body = $"Hello {fullName},<br/><br/>Please verify your email by clicking: <a href='{verificationLink}'>Verify</a>";
            return await SendEmailAsync(email, "EduTrack - Verify Email", body);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetLink, string fullName)
        {
            string body = $"Hello {fullName},<br/><br/>Reset your password: <a href='{resetLink}'>Reset Password</a><br/>This link expires in 1 hour.";
            return await SendEmailAsync(email, "EduTrack - Password Reset", body);
        }

        public async Task<bool> SendReportEmailAsync(string email, string reportName, byte[] reportData, string format)
        {
            string ext = format == "pdf" ? "pdf" : format == "excel" ? "xlsx" : "doc";
            return await SendEmailWithAttachmentAsync(email, $"EduTrack - {reportName} Report", $"Attached {reportName} report.", reportData, $"{reportName}_{System.DateTime.Now:yyyyMMdd}.{ext}");
        }
    }
}