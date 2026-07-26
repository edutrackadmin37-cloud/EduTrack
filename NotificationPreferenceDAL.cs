// ============================================================
// DAL/NotificationPreferenceDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class NotificationPreferenceDAL : BaseDAL
    {
        public NotificationPreference GetByUser(int userId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetNotificationPreference", new SqlParameter("@UserID", userId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(NotificationPreference model)
        {
            SqlParameter outId = new SqlParameter("@NewPreferenceID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateNotificationPreference",
                new SqlParameter("@UserID", model.UserID),
                new SqlParameter("@EmailNotifications", model.EmailNotifications),
                new SqlParameter("@SmsNotifications", model.SmsNotifications),
                new SqlParameter("@InAppNotifications", model.InAppNotifications),
                new SqlParameter("@ProjectUpdates", model.ProjectUpdates),
                new SqlParameter("@GradeAlerts", model.GradeAlerts),
                new SqlParameter("@AttendanceAlerts", model.AttendanceAlerts),
                new SqlParameter("@MessageAlerts", model.MessageAlerts),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(NotificationPreference model)
        {
            int rows = ExecuteNonQuery("sp_UpdateNotificationPreference",
                new SqlParameter("@PreferenceID", model.PreferenceID),
                new SqlParameter("@EmailNotifications", model.EmailNotifications),
                new SqlParameter("@SmsNotifications", model.SmsNotifications),
                new SqlParameter("@InAppNotifications", model.InAppNotifications),
                new SqlParameter("@ProjectUpdates", model.ProjectUpdates),
                new SqlParameter("@GradeAlerts", model.GradeAlerts),
                new SqlParameter("@AttendanceAlerts", model.AttendanceAlerts),
                new SqlParameter("@MessageAlerts", model.MessageAlerts));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteNotificationPreference", new SqlParameter("@PreferenceID", id)) > 0;
        }

        private NotificationPreference Map(SqlDataReader r)
        {
            return new NotificationPreference
            {
                PreferenceID = GetValue<int>(r, "PreferenceID"),
                UserID = GetValue<int>(r, "UserID"),
                EmailNotifications = GetValue<bool>(r, "EmailNotifications"),
                SmsNotifications = GetValue<bool>(r, "SmsNotifications"),
                InAppNotifications = GetValue<bool>(r, "InAppNotifications"),
                ProjectUpdates = GetValue<bool>(r, "ProjectUpdates"),
                GradeAlerts = GetValue<bool>(r, "GradeAlerts"),
                AttendanceAlerts = GetValue<bool>(r, "AttendanceAlerts"),
                MessageAlerts = GetValue<bool>(r, "MessageAlerts"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}