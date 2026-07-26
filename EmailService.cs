using System.Threading.Tasks;

namespace EduTrack.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string bodyHtml);
        Task<bool> SendEmailAsync(string to, string subject, string bodyHtml, string from = null);
        Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string bodyHtml, byte[] attachment, string attachmentName);
        Task<bool> SendVerificationEmailAsync(string email, string verificationLink, string fullName);
        Task<bool> SendPasswordResetEmailAsync(string email, string resetLink, string fullName);
        Task<bool> SendReportEmailAsync(string email, string reportName, byte[] reportData, string format);
    }
}