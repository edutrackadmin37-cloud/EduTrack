// ============================================================
// DAL/DiscussionBoardDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class DiscussionBoardDAL : BaseDAL
    {
        public List<DiscussionBoard> GetAll()
        {
            var list = new List<DiscussionBoard>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllDiscussions"))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<DiscussionBoard> GetBySubject(int subjectId)
        {
            var list = new List<DiscussionBoard>();
            using (SqlDataReader r = ExecuteReader("sp_GetDiscussionsBySubject", new SqlParameter("@SubjectID", subjectId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<DiscussionBoard> GetByClass(int classId)
        {
            var list = new List<DiscussionBoard>();
            using (SqlDataReader r = ExecuteReader("sp_GetDiscussionsByClass", new SqlParameter("@ClassID", classId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<DiscussionBoard> GetByProject(int projectId)
        {
            var list = new List<DiscussionBoard>();
            using (SqlDataReader r = ExecuteReader("sp_GetDiscussionsByProject", new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public DiscussionBoard GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetDiscussionById", new SqlParameter("@DiscussionID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public int Create(DiscussionBoard model)
        {
            SqlParameter outId = new SqlParameter("@NewDiscussionID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateDiscussion",
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@ClassID", (object)model.ClassID ?? DBNull.Value),
                new SqlParameter("@ProjectID", (object)model.ProjectID ?? DBNull.Value),
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Body", (object)model.Body ?? DBNull.Value),
                new SqlParameter("@PostedBy", model.PostedBy),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(DiscussionBoard model)
        {
            int rows = ExecuteNonQuery("sp_UpdateDiscussion",
                new SqlParameter("@DiscussionID", model.DiscussionID),
                new SqlParameter("@Title", (object)model.Title ?? DBNull.Value),
                new SqlParameter("@Body", (object)model.Body ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteDiscussion", new SqlParameter("@DiscussionID", id)) > 0;
        }

        private DiscussionBoard Map(SqlDataReader r)
        {
            return new DiscussionBoard
            {
                DiscussionID = GetValue<int>(r, "DiscussionID"),
                SubjectID = GetValue<int>(r, "SubjectID"),
                ClassID = GetValue<int?>(r, "ClassID"),
                ProjectID = GetValue<int?>(r, "ProjectID"),
                Title = GetValue<string>(r, "Title"),
                Body = GetValue<string>(r, "Body"),
                PostedBy = GetValue<int>(r, "PostedBy"),
                PostedDate = GetValue<DateTime>(r, "PostedDate"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private DiscussionBoard MapWithContext(SqlDataReader r)
        {
            var d = Map(r);
            d.SubjectName = GetValue<string>(r, "SubjectName");
            d.PostedByName = GetValue<string>(r, "PostedByName");
            d.ClassName = GetValue<string>(r, "ClassName");
            d.ProjectTitle = GetValue<string>(r, "ProjectTitle");
            return d;
        }
    }
}