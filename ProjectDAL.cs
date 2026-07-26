using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ProjectDAL : BaseDAL
    {
        public List<Project> GetAll()
        {
            List<Project> list = new List<Project>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllProjects"))
            {
                while (r.Read()) list.Add(MapWithContext(r));
            }
            return list;
        }
        public List<Project> GetByClassSubjectTeacher(int classSubjectTeacherId)
        {
            List<Project> list = new List<Project>();
            using (SqlDataReader r = ExecuteReader("sp_GetProjectsByClassSubjectTeacher", new SqlParameter("@ClassSubjectTeacherID", classSubjectTeacherId)))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }
        public Project GetById(int projectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetProjectById", new SqlParameter("@ProjectID", projectId)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        //public List<Project> GetByClassSubjectTeacher(int classSubjectTeacherId)
        //{
        //    List<Project> list = new List<Project>();
        //    using (SqlDataReader r = ExecuteReader("sp_GetProjectsByClassSubjectTeacher", new SqlParameter("@ClassSubjectTeacherID", classSubjectTeacherId)))
        //    {
        //        while (r.Read()) list.Add(Map(r));
        //    }
        //    return list;
        //}

        public int Create(Project model)
        {
            SqlParameter outId = new SqlParameter("@NewProjectID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateProject",
                new SqlParameter("@ClassSubjectTeacherID", model.ClassSubjectTeacherID),
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@Objectives", (object)model.Objectives ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@MaxTeamSize", model.MaxTeamSize),
                new SqlParameter("@AllowTeamFormation", model.AllowTeamFormation),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@CreatedBy", model.CreatedBy),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Project model)
        {
            int rows = ExecuteNonQuery("sp_UpdateProject",
                new SqlParameter("@ProjectID", model.ProjectID),
                new SqlParameter("@Title", (object)model.Title ?? DBNull.Value),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@Objectives", (object)model.Objectives ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@MaxTeamSize", model.MaxTeamSize),
                new SqlParameter("@AllowTeamFormation", model.AllowTeamFormation),
                new SqlParameter("@Status", (object)model.Status ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool UpdateStatus(int projectId, string newStatus, int changedBy, string comments)
        {
            int rows = ExecuteNonQuery("sp_UpdateProjectStatus",
                new SqlParameter("@ProjectID", projectId),
                new SqlParameter("@NewStatus", newStatus),
                new SqlParameter("@ChangedBy", changedBy),
                new SqlParameter("@Comments", (object)comments ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int projectId)
        {
            return ExecuteNonQuery("sp_SoftDeleteProject", new SqlParameter("@ProjectID", projectId)) > 0;
        }

        private Project Map(SqlDataReader r)
        {
            return new Project
            {
                ProjectID = GetValue<int>(r, "ProjectID"),
                ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                Title = GetValue<string>(r, "Title"),
                Description = GetValue<string>(r, "Description"),
                Objectives = GetValue<string>(r, "Objectives"),
                StartDate = GetValue<DateTime?>(r, "StartDate"),
                EndDate = GetValue<DateTime?>(r, "EndDate"),
                MaxTeamSize = GetValue<int>(r, "MaxTeamSize"),
                AllowTeamFormation = GetValue<bool>(r, "AllowTeamFormation"),
                Status = GetValue<string>(r, "Status"),
                CreatedBy = GetValue<int>(r, "CreatedBy"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private Project MapWithContext(SqlDataReader r)
        {
            Project p = Map(r);
            p.ClassID = GetValue<int?>(r, "ClassID");
            p.SubjectID = GetValue<int?>(r, "SubjectID");
            p.TeacherID = GetValue<int?>(r, "TeacherID");
            p.CreatedByName = HasColumn(r, "CreatedByName") ? GetValue<string>(r, "CreatedByName") : null;
            return p;
        }

        private bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}