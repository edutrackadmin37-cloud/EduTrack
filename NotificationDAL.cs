using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class NotificationDAL : BaseDAL
    {
        public List<Notification> GetByUser(int userId)
        {
            List<Notification> list = new List<Notification>();
            using (SqlDataReader r = ExecuteReader("sp_GetNotificationsByUser", new SqlParameter("@UserID", userId)))
            {
                while (r.Read())
                {
                    list.Add(new Notification
                    {
                        NotificationID = GetValue<int>(r, "NotificationID"),
                        UserID = GetValue<int>(r, "UserID"),
                        NotificationText = GetValue<string>(r, "NotificationText"),
                        NotificationDate = GetValue<DateTime>(r, "NotificationDate"),
                        IsRead = GetValue<bool>(r, "IsRead"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public Notification GetById(int notificationId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetNotificationById", new SqlParameter("@NotificationID", notificationId)))
            {
                if (!r.Read()) return null;
                return new Notification
                {
                    NotificationID = GetValue<int>(r, "NotificationID"),
                    UserID = GetValue<int>(r, "UserID"),
                    NotificationText = GetValue<string>(r, "NotificationText"),
                    NotificationDate = GetValue<DateTime>(r, "NotificationDate"),
                    IsRead = GetValue<bool>(r, "IsRead"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(Notification model)
        {
            SqlParameter outId = new SqlParameter("@NewNotificationID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateNotification",
                new SqlParameter("@UserID", model.UserID),
                new SqlParameter("@NotificationText", model.NotificationText),
                new SqlParameter("@IsRead", model.IsRead),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool MarkAsRead(int notificationId) => ExecuteNonQuery("sp_MarkNotificationAsRead", new SqlParameter("@NotificationID", notificationId)) > 0;
        public bool SoftDelete(int notificationId) => ExecuteNonQuery("sp_SoftDeleteNotification", new SqlParameter("@NotificationID", notificationId)) > 0;

        public bool MarkAllAsRead(int userId)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };
            int affected = ExecuteNonQuery("sp_Notification_MarkAllAsRead", parameters);
            return affected > 0;
        }
    }
}