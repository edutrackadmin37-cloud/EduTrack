using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace EduTrack.Services
{
    public class HubtelSmsService : ISmsService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _from;
        private readonly HttpClient _httpClient;
        private readonly JavaScriptSerializer _json;

        public HubtelSmsService()
        {
            _clientId = ConfigurationManager.AppSettings["HubtelClientId"];
            _clientSecret = ConfigurationManager.AppSettings["HubtelClientSecret"];
            _from = ConfigurationManager.AppSettings["HubtelFrom"] ?? "EduTrack";
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.hubtel.com/v1/") };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            _json = new JavaScriptSerializer();
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                var payload = new { from = _from, to = phoneNumber, content = message };
                var content = new StringContent(_json.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("messages/send", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> SendVerificationSmsAsync(string phoneNumber, string code)
            => await SendSmsAsync(phoneNumber, $"Your EduTrack code: {code}");

        public async Task<bool> SendParentNotificationAsync(string phoneNumber, string studentName, string message)
        {
            var fullMessage = $"Dear Parent, {studentName}: {message}";
            return await SendSmsAsync(phoneNumber, fullMessage);
        }

        public async Task<bool> SendPerformanceAlertAsync(string phoneNumber, string studentName, string subject, string grade)
        {
            var message = $"Performance Alert: {studentName} scored {grade} in {subject}.";
            return await SendSmsAsync(phoneNumber, message);
        }

        public async Task<bool> SendAttendanceAlertAsync(string phoneNumber, string studentName, string className, string status)
        {
            var message = $"Attendance Alert: {studentName} was marked {status} in {className}.";
            return await SendSmsAsync(phoneNumber, message);
        }

        public async Task<bool> SendBulkSmsAsync(string[] phoneNumbers, string message)
        {
            var tasks = new Task<bool>[phoneNumbers.Length];
            for (var i = 0; i < phoneNumbers.Length; i++)
            {
                tasks[i] = SendSmsAsync(phoneNumbers[i], message);
            }
            var results = await Task.WhenAll(tasks);
            return Array.TrueForAll(results, r => r);
        }
    }
}