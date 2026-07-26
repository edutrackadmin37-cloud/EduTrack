// ============================================================
// DAL/ActivityLogDAL.cs (Refactored – no inline SQL)
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ActivityLogDAL : BaseDAL
    {
        public List<ActivityLog> GetActivityLogs(int? userId = null, string action = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = new List<ActivityLog>();
            using (SqlDataReader r = ExecuteReader("sp_GetActivityLogs",
                new SqlParameter("@UserID", (object)userId ?? DBNull.Value),
                new SqlParameter("@Action", (object)action ?? DBNull.Value),
                new SqlParameter("@FromDate", (object)fromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object)toDate ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new ActivityLog
                    {
                        LogID = GetValue<int>(r, "LogID"),
                        UserID = GetValue<int>(r, "UserID"),
                        Action = GetValue<string>(r, "Action"),
                        ActionDate = GetValue<DateTime>(r, "ActionDate"),
                        IPAddress = GetValue<string>(r, "IPAddress"),
                        Details = GetValue<string>(r, "Details"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        FullName = HasColumn(r, "FullName") ? GetValue<string>(r, "FullName") : null
                    });
                }
            }
            return list;
        }

        public int CreateActivityLog(ActivityLog log)
        {
            SqlParameter outId = new SqlParameter("@NewLogID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateActivityLog",
                new SqlParameter("@UserID", log.UserID),
                new SqlParameter("@Action", log.Action),
                new SqlParameter("@IPAddress", (object)log.IPAddress ?? DBNull.Value),
                new SqlParameter("@Details", (object)log.Details ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public List<string> GetDistinctActions()
        {
            var actions = new List<string>();
            using (SqlDataReader r = ExecuteReader("sp_GetDistinctActions"))
            {
                while (r.Read())
                {
                    actions.Add(GetValue<string>(r, "Action"));
                }
            }
            return actions;
        }

        public DataTable GetAuditLog(int? userId, string action, DateTime? date)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetAuditLog", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", (object)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Action", (object)action ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Date", (object)date ?? DBNull.Value);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private bool HasColumn(SqlDataReader r, string column)
        {
            for (int i = 0; i < r.FieldCount; i++)
                if (r.GetName(i).Equals(column, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}