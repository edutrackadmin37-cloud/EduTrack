using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace EduTrack.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;

        public TwilioSmsService()
        {
            _accountSid = System.Configuration.ConfigurationManager.AppSettings["TwilioAccountSid"] ?? "";
            _authToken = System.Configuration.ConfigurationManager.AppSettings["TwilioAuthToken"] ?? "";
            _fromNumber = System.Configuration.ConfigurationManager.AppSettings["TwilioFromNumber"] ?? "+1234567890";
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            if (string.IsNullOrWhiteSpace(_accountSid) || string.IsNullOrWhiteSpace(_authToken))
                return false;

            try
            {
                TwilioClient.Init(_accountSid, _authToken);
                var msg = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(_fromNumber),
                    to: new PhoneNumber(phoneNumber)
                );
                return msg.Status == MessageResource.StatusEnum.Accepted ||
                       msg.Status == MessageResource.StatusEnum.Queued ||
                       msg.Status == MessageResource.StatusEnum.Sent;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendBulkSmsAsync(string[] phoneNumbers, string message)
        {
            bool allSent = true;
            foreach (var number in phoneNumbers)
            {
                if (!string.IsNullOrWhiteSpace(number))
                {
                    var result = await SendSmsAsync(number, message);
                    if (!result) allSent = false;
                }
            }
            return allSent;
        }

        public async Task<bool> SendAttendanceAlertAsync(string phoneNumber, string studentName, string className, string status)
        {
            string msg = $"EduTrack Attendance Alert: {studentName} was marked {status} in {className} on {DateTime.Now:dd-MMM-yyyy}.";
            return await SendSmsAsync(phoneNumber, msg);
        }

        public async Task<bool> SendPerformanceAlertAsync(string phoneNumber, string studentName, string subject, string grade)
        {
            string msg = $"EduTrack Performance Alert: {studentName} scored {grade} in {subject}. Check EduTrack for details.";
            return await SendSmsAsync(phoneNumber, msg);
        }

        public async Task<bool> SendParentNotificationAsync(string phoneNumber, string studentName, string message)
        {
            string msg = $"EduTrack Notification for {studentName}: {message}";
            return await SendSmsAsync(phoneNumber, msg);
        }
    }
}