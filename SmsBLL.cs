using EduTrack.Services;
using System.Threading.Tasks;

namespace EduTrack.BLL
{
    public class SmsBLL
    {
        private readonly ISmsService _smsService;

        public SmsBLL()
        {
            _smsService = new TwilioSmsService();
        }

        public async Task<bool> SendSms(string phone, string message)
        {
            return await _smsService.SendSmsAsync(phone, message);
        }

        public async Task<bool> SendAttendanceAlert(string phone, string studentName, string className, string status)
        {
            return await _smsService.SendAttendanceAlertAsync(phone, studentName, className, status);
        }

        public async Task<bool> SendPerformanceAlert(string phone, string studentName, string subject, string grade)
        {
            return await _smsService.SendPerformanceAlertAsync(phone, studentName, subject, grade);
        }
    }
}