using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class StreamDAL : BaseDAL
    {
        public List<Stream> GetAll()
        {
            List<Stream> list = new List<Stream>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllStreams"))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Stream GetById(int streamId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetStreamById", new SqlParameter("@StreamID", streamId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Stream stream)
        {
            SqlParameter outId = new SqlParameter("@NewStreamID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateStream",
                new SqlParameter("@StreamName", stream.StreamName),
                new SqlParameter("@Description", (object)stream.Description ?? DBNull.Value),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Stream stream)
        {
            int rows = ExecuteNonQuery("sp_UpdateStream",
                new SqlParameter("@StreamID", stream.StreamID),
                new SqlParameter("@StreamName", (object)stream.StreamName ?? DBNull.Value),
                new SqlParameter("@Description", (object)stream.Description ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int streamId)
        {
            return ExecuteNonQuery("sp_SoftDeleteStream", new SqlParameter("@StreamID", streamId)) > 0;
        }

        private Stream Map(SqlDataReader r)
        {
            return new Stream
            {
                StreamID = GetValue<int>(r, "StreamID"),
                StreamName = GetValue<string>(r, "StreamName"),
                Description = GetValue<string>(r, "Description"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}