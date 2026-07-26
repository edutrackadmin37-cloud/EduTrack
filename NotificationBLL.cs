using EduTrack.DAL;
using EduTrack.Models;
using EduTrack.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace EduTrack.BLL
{
    public class NotificationBLL
    {
        private readonly NotificationDAL _dal = new NotificationDAL();
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly bool _emailEnabled;
        private readonly bool _smsEnabled;

        public NotificationBLL()
        {
            _emailEnabled = bool.TryParse(ConfigurationManager.AppSettings["EnableEmail"], out bool e) && e;
            _smsEnabled = bool.TryParse(ConfigurationManager.AppSettings["EnableSms"], out bool s) && s;

            if (_emailEnabled)
                _emailService = new SendGridEmailService();
            else
                _emailService = new EmailService(); // fallback SMTP

            if (_smsEnabled)
                _smsService = new TwilioSmsService();
        }

        public Response<List<Notification>> GetByUser(int userId)
        {
            if (userId <= 0) return Response<List<Notification>>.Failure("Invalid user ID.", "VALIDATION_ERROR");
            return Response<List<Notification>>.Success(_dal.GetByUser(userId));
        }

        public Response<int> Create(Notification notification)
        {
            if (notification == null || notification.UserID <= 0 || string.IsNullOrWhiteSpace(notification.NotificationText))
                return Response<int>.Failure("Invalid notification data.", "VALIDATION_ERROR");

            int id = _dal.Create(notification);
            return id > 0 ? Response<int>.Success(id, "Notification created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public async Task<Response<int>> CreateAndSendAsync(Notification notification, string email = null, string phone = null)
        {
            if (notification == null || notification.UserID <= 0 || string.IsNullOrWhiteSpace(notification.NotificationText))
                return Response<int>.Failure("Invalid notification data.", "VALIDATION_ERROR");

            int id = _dal.Create(notification);

            if (!string.IsNullOrWhiteSpace(email) && _emailEnabled)
            {
                await _emailService.SendEmailAsync(email, "EduTrack Notification", notification.NotificationText);
            }

            if (!string.IsNullOrWhiteSpace(phone) && _smsEnabled)
            {
                await _smsService.SendSmsAsync(phone, notification.NotificationText);
            }

            return id > 0
                ? Response<int>.Success(id, "Notification sent.")
                : Response<int>.Failure("Failed to send notification.", "CREATE_FAILED");
        }

        public Response<bool> MarkAsRead(int notificationId)
        {
            if (notificationId <= 0) return Response<bool>.Failure("Invalid notification ID.", "VALIDATION_ERROR");
            bool ok = _dal.MarkAsRead(notificationId);
            return ok ? Response<bool>.Success(true, "Notification marked as read.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> MarkAllAsRead(int userId)
        {
            if (userId <= 0) return Response<bool>.Failure("Invalid user ID.", "VALIDATION_ERROR");
            bool ok = _dal.MarkAllAsRead(userId);
            return ok ? Response<bool>.Success(true, "All notifications marked as read.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDelete(int notificationId)
        {
            if (notificationId <= 0) return Response<bool>.Failure("Invalid notification ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDelete(notificationId);
            return ok ? Response<bool>.Success(true, "Notification deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}