using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EduTrack.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SendGridEmailService()
        {
            _apiKey = System.Configuration.ConfigurationManager.AppSettings["SendGridApiKey"] ?? "";
            _fromEmail = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"] ?? "noreply@edutrack.com";
            _fromName = System.Configuration.ConfigurationManager.AppSettings["EmailFromName"] ?? "EduTrack System";
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string bodyHtml)
        {
            return await SendEmailAsync(to, subject, bodyHtml, _fromEmail);
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string bodyHtml, string from = null)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return false;

            var client = new SendGridClient(_apiKey);
            var fromEmail = new EmailAddress(from ?? _fromEmail, _fromName);
            var toEmail = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, bodyHtml, bodyHtml);
            var response = await client.SendEmailAsync(msg);
            return response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                   response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string bodyHtml, byte[] attachment, string attachmentName)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return false;

            var client = new SendGridClient(_apiKey);
            var fromEmail = new EmailAddress(_fromEmail, _fromName);
            var toEmail = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, bodyHtml, bodyHtml);

            if (attachment != null && attachment.Length > 0)
            {
                var att = new Attachment
                {
                    Content = Convert.ToBase64String(attachment),
                    Filename = attachmentName,
                    Type = "application/pdf"
                };
                msg.Attachments = new System.Collections.Generic.List<Attachment> { att };
            }

            var response = await client.SendEmailAsync(msg);
            return response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                   response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> SendVerificationEmailAsync(string email, string verificationLink, string fullName)
        {
            string subject = "EduTrack - Verify Your Email";
            string body = $@"
                <html><body style='font-family: Arial, sans-serif;'>
                <h2>Welcome to EduTrack, {fullName}!</h2>
                <p>Please verify your email address by clicking the link below:</p>
                <p><a href='{verificationLink}' style='background:#667eea;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>Verify Email</a></p>
                <p>If you did not create an account, please ignore this email.</p>
                <br/><p>Regards,<br/>EduTrack Team</p>
                </body></html>";
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetLink, string fullName)
        {
            string subject = "EduTrack - Password Reset";
            string body = $@"
                <html><body style='font-family: Arial, sans-serif;'>
                <h2>Password Reset Request</h2>
                <p>Hello {fullName},</p>
                <p>Click the link below to reset your password:</p>
                <p><a href='{resetLink}' style='background:#667eea;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>Reset Password</a></p>
                <p>This link expires in 1 hour.</p>
                <br/><p>Regards,<br/>EduTrack Team</p>
                </body></html>";
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendReportEmailAsync(string email, string reportName, byte[] reportData, string format)
        {
            string subject = $"EduTrack - {reportName} Report";
            string body = $@"
                <html><body style='font-family: Arial, sans-serif;'>
                <h2>Your {reportName} Report</h2>
                <p>Please find attached the {reportName} report generated from EduTrack.</p>
                <br/><p>Regards,<br/>EduTrack Team</p>
                </body></html>";
            string ext = format == "pdf" ? "pdf" : format == "excel" ? "xlsx" : "doc";
            string mime = format == "pdf" ? "application/pdf" : format == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/msword";
            return await SendEmailWithAttachmentAsync(email, subject, body, reportData, $"{reportName}_{DateTime.Now:yyyyMMdd}.{ext}");
        }
    }
}