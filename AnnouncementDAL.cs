using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class AnnouncementDAL : BaseDAL
    {
        public List<Announcement> GetAllAnnouncements()
        {
            List<Announcement> list = new List<Announcement>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllAnnouncements"))
            {
                while (r.Read()) list.Add(MapWithContext(r));
            }
            return list;
        }

        public List<Announcement> GetAnnouncementsByClass(int classId)
        {
            List<Announcement> list = new List<Announcement>();
            using (SqlDataReader r = ExecuteReader("sp_GetAnnouncementsByClass", new SqlParameter("@ClassID", classId)))
            {
                while (r.Read()) list.Add(MapWithPostedBy(r));
            }
            return list;
        }

        public Announcement GetAnnouncementById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetAnnouncementById", new SqlParameter("@AnnouncementID", id)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int CreateAnnouncement(Announcement announcement)
        {
            SqlParameter outId = new SqlParameter("@NewAnnouncementID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateAnnouncement",
                new SqlParameter("@Title", announcement.Title),
                new SqlParameter("@Body", (object)announcement.Body ?? DBNull.Value),
                new SqlParameter("@PostedBy", announcement.PostedBy),
                new SqlParameter("@TargetClassID", (object)announcement.TargetClassID ?? DBNull.Value),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool UpdateAnnouncement(Announcement announcement)
        {
            int rows = ExecuteNonQuery("sp_UpdateAnnouncement",
                new SqlParameter("@AnnouncementID", announcement.AnnouncementID),
                new SqlParameter("@Title", (object)announcement.Title ?? DBNull.Value),
                new SqlParameter("@Body", (object)announcement.Body ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDeleteAnnouncement(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteAnnouncement", new SqlParameter("@AnnouncementID", id)) > 0;
        }

        private Announcement Map(SqlDataReader r)
        {
            return new Announcement
            {
                AnnouncementID = GetValue<int>(r, "AnnouncementID"),
                Title = GetValue<string>(r, "Title"),
                Body = GetValue<string>(r, "Body"),
                PostedBy = GetValue<int>(r, "PostedBy"),
                PostedDate = GetValue<DateTime>(r, "PostedDate"),
                TargetClassID = GetValue<int?>(r, "TargetClassID"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private Announcement MapWithContext(SqlDataReader r)
        {
            Announcement a = Map(r);
            a.PostedByName = GetValue<string>(r, "PostedByName");
            a.TargetClassName = GetValue<string>(r, "TargetClassName");
            return a;
        }

        private Announcement MapWithPostedBy(SqlDataReader r)
        {
            Announcement a = Map(r);
            a.PostedByName = GetValue<string>(r, "PostedByName");
            return a;
        }
    }
}