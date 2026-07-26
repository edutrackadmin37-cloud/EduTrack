using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class MessageDAL : BaseDAL
    {
        public List<Message> GetByUser(int userId)
        {
            List<Message> list = new List<Message>();
            using (SqlDataReader r = ExecuteReader("sp_GetMessagesByUser", new SqlParameter("@UserID", userId)))
            {
                while (r.Read())
                {
                    list.Add(new Message
                    {
                        MessageID = GetValue<int>(r, "MessageID"),
                        SenderID = GetValue<int>(r, "SenderID"),
                        ReceiverID = GetValue<int>(r, "ReceiverID"),
                        Subject = GetValue<string>(r, "Subject"),
                        Body = GetValue<string>(r, "Body"),
                        SentDate = GetValue<DateTime>(r, "SentDate"),
                        IsRead = GetValue<bool>(r, "IsRead"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        SenderName = GetValue<string>(r, "SenderName"),
                        ReceiverName = GetValue<string>(r, "ReceiverName")
                    });
                }
            }
            return list;
        }

        public Message GetById(int messageId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetMessageById", new SqlParameter("@MessageID", messageId)))
            {
                if (!r.Read()) return null;
                return new Message
                {
                    MessageID = GetValue<int>(r, "MessageID"),
                    SenderID = GetValue<int>(r, "SenderID"),
                    ReceiverID = GetValue<int>(r, "ReceiverID"),
                    Subject = GetValue<string>(r, "Subject"),
                    Body = GetValue<string>(r, "Body"),
                    SentDate = GetValue<DateTime>(r, "SentDate"),
                    IsRead = GetValue<bool>(r, "IsRead"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(Message model)
        {
            SqlParameter outId = new SqlParameter("@NewMessageID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateMessage",
                new SqlParameter("@SenderID", model.SenderID),
                new SqlParameter("@ReceiverID", model.ReceiverID),
                new SqlParameter("@Subject", (object)model.Subject ?? DBNull.Value),
                new SqlParameter("@Body", (object)model.Body ?? DBNull.Value),
                new SqlParameter("@IsRead", model.IsRead),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool MarkAsRead(int messageId) => ExecuteNonQuery("sp_MarkMessageAsRead", new SqlParameter("@MessageID", messageId)) > 0;
        public bool SoftDelete(int messageId) => ExecuteNonQuery("sp_SoftDeleteMessage", new SqlParameter("@MessageID", messageId)) > 0;
    }
}