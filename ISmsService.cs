using System.Threading.Tasks;

namespace EduTrack.Services
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
        Task<bool> SendBulkSmsAsync(string[] phoneNumbers, string message);
        Task<bool> SendAttendanceAlertAsync(string phoneNumber, string studentName, string className, string status);
        Task<bool> SendPerformanceAlertAsync(string phoneNumber, string studentName, string subject, string grade);
        Task<bool> SendParentNotificationAsync(string phoneNumber, string studentName, string message);
    }
}