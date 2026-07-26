// ============================================================
// DAL/BaseDAL.cs (Minor improvements)
// ============================================================
using System;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public abstract class BaseDAL
    {
        protected int ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        protected object ExecuteScalar(string storedProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        protected SqlDataReader ExecuteReader(string storedProcedure, params SqlParameter[] parameters)
        {
            SqlConnection conn = DbConnection.GetConnection();
            SqlCommand cmd = new SqlCommand(storedProcedure, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        protected DataTable ExecuteDataTable(string storedProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        protected static T GetValue<T>(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal)) return default(T);
            return (T)reader.GetValue(ordinal);
        }
    }
}