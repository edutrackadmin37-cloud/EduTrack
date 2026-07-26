using System;
using System.IO;
using System.Threading.Tasks;

namespace EduTrack.Services
{
    public class SmsServiceStub : ISmsService
    {
        private readonly string _logPath;

        public SmsServiceStub()
        {
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs");
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
            _logPath = Path.Combine(logDir, "sms_log.txt");
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | TO: {phoneNumber} | {message}\n{new string('-', 80)}\n";
            File.AppendAllText(_logPath, entry);
            return await Task.FromResult(true);
        }

        public async Task<bool> SendBulkSmsAsync(string[] phoneNumbers, string message)
        {
            var allSucceeded = true;
            foreach (var phoneNumber in phoneNumbers)
            {
                var result = await SendSmsAsync(phoneNumber, message);
                if (!result) allSucceeded = false;
            }
            return allSucceeded;
        }

        public async Task<bool> SendAttendanceAlertAsync(string phoneNumber, string studentName, string className, string status)
            => await SendSmsAsync(phoneNumber, $"Attendance Alert: {studentName} in {className} is marked as {status}.");

        public async Task<bool> SendPerformanceAlertAsync(string phoneNumber, string studentName, string subject, string grade)
            => await SendSmsAsync(phoneNumber, $"Performance Alert: {studentName} received {grade} in {subject}.");

        public async Task<bool> SendParentNotificationAsync(string phoneNumber, string studentName, string message)
            => await SendSmsAsync(phoneNumber, $"Parent Notification for {studentName}: {message}");
    }
}