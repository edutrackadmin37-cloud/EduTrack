using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class AssignmentDAL : BaseDAL
    {
        public List<Assignment> GetAll()
        {
            List<Assignment> list = new List<Assignment>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllAssignments"))
            {
                while (r.Read()) list.Add(MapWithContext(r));
            }
            return list;
        }

        public Assignment GetById(int assignmentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetAssignmentById", new SqlParameter("@AssignmentID", assignmentId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public List<Assignment> GetByClassSubjectTeacher(int classSubjectTeacherId)
        {
            List<Assignment> list = new List<Assignment>();
            using (SqlDataReader r = ExecuteReader("sp_GetAssignmentsByClassSubjectTeacher", new SqlParameter("@ClassSubjectTeacherID", classSubjectTeacherId)))
            {
                while (r.Read()) list.Add(MapWithRubric(r));
            }
            return list;
        }

        public int Create(Assignment model)
        {
            SqlParameter outId = new SqlParameter("@NewAssignmentID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateAssignment",
                new SqlParameter("@ProjectID", (object)model.ProjectID ?? DBNull.Value),
                new SqlParameter("@ClassSubjectTeacherID", model.ClassSubjectTeacherID),
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@AssignedDate", (object)model.AssignedDate ?? DBNull.Value),
                new SqlParameter("@DueDate", (object)model.DueDate ?? DBNull.Value),
                new SqlParameter("@RubricID", model.RubricID),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Assignment model)
        {
            int rows = ExecuteNonQuery("sp_UpdateAssignment",
                new SqlParameter("@AssignmentID", model.AssignmentID),
                new SqlParameter("@ProjectID", (object)model.ProjectID ?? DBNull.Value),
                new SqlParameter("@Title", (object)model.Title ?? DBNull.Value),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@AssignedDate", (object)model.AssignedDate ?? DBNull.Value),
                new SqlParameter("@DueDate", (object)model.DueDate ?? DBNull.Value),
                new SqlParameter("@RubricID", model.RubricID)
            );
            return rows > 0;
        }

        public bool SoftDelete(int assignmentId)
        {
            return ExecuteNonQuery("sp_SoftDeleteAssignment", new SqlParameter("@AssignmentID", assignmentId)) > 0;
        }

        private Assignment Map(SqlDataReader r)
        {
            return new Assignment
            {
                AssignmentID = GetValue<int>(r, "AssignmentID"),
                ProjectID = GetValue<int?>(r, "ProjectID"),
                ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                Title = GetValue<string>(r, "Title"),
                Description = GetValue<string>(r, "Description"),
                AssignedDate = GetValue<DateTime?>(r, "AssignedDate"),
                DueDate = GetValue<DateTime?>(r, "DueDate"),
                RubricID = GetValue<int>(r, "RubricID"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private Assignment MapWithRubric(SqlDataReader r)
        {
            Assignment a = Map(r);
            a.RubricTitle = GetValue<string>(r, "RubricTitle");
            return a;
        }

        private Assignment MapWithContext(SqlDataReader r)
        {
            Assignment a = Map(r);
            a.ClassID = GetValue<int?>(r, "ClassID");
            a.SubjectID = GetValue<int?>(r, "SubjectID");
            a.TeacherID = GetValue<int?>(r, "TeacherID");
            a.RubricTitle = GetValue<string>(r, "RubricTitle");
            return a;
        }
    }
}