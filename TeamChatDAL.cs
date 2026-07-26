// ============================================================
// DAL/TeamChatDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TeamChatDAL : BaseDAL
    {
        public List<TeamChatMessage> GetByTeam(int teamId)
        {
            var list = new List<TeamChatMessage>();
            using (SqlDataReader r = ExecuteReader("sp_GetTeamChatMessages", new SqlParameter("@TeamID", teamId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public TeamChatMessage GetById(int messageId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetTeamChatMessageById", new SqlParameter("@MessageID", messageId)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public int Create(TeamChatMessage model)
        {
            SqlParameter outId = new SqlParameter("@NewMessageID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateTeamChatMessage",
                new SqlParameter("@TeamID", model.TeamID),
                new SqlParameter("@SenderID", model.SenderID),
                new SqlParameter("@MessageText", model.MessageText),
                new SqlParameter("@FilePath", (object)model.FilePath ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool MarkAsRead(int messageId)
        {
            return ExecuteNonQuery("sp_MarkTeamChatMessageRead", new SqlParameter("@MessageID", messageId)) > 0;
        }

        public bool SoftDelete(int messageId)
        {
            return ExecuteNonQuery("sp_SoftDeleteTeamChatMessage", new SqlParameter("@MessageID", messageId)) > 0;
        }

        private TeamChatMessage Map(SqlDataReader r)
        {
            return new TeamChatMessage
            {
                MessageID = GetValue<int>(r, "MessageID"),
                TeamID = GetValue<int>(r, "TeamID"),
                SenderID = GetValue<int>(r, "SenderID"),
                MessageText = GetValue<string>(r, "MessageText"),
                FilePath = GetValue<string>(r, "FilePath"),
                SentDate = GetValue<DateTime>(r, "SentDate"),
                IsRead = GetValue<bool>(r, "IsRead"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private TeamChatMessage MapWithContext(SqlDataReader r)
        {
            var msg = Map(r);
            msg.SenderName = GetValue<string>(r, "SenderName");
            msg.TeamName = GetValue<string>(r, "TeamName");
            return msg;
        }
    }
}