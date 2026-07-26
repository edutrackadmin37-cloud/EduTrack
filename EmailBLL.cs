using EduTrack.Services;
using System.Threading.Tasks;

namespace EduTrack.BLL
{
    public class EmailBLL
    {
        private readonly IEmailService _emailService;

        public EmailBLL()
        {
            _emailService = new SendGridEmailService();
        }

        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            return await _emailService.SendEmailAsync(to, subject, body);
        }

        public async Task<bool> SendReport(string to, string reportName, byte[] data, string format)
        {
            return await _emailService.SendReportEmailAsync(to, reportName, data, format);
        }
    }
}