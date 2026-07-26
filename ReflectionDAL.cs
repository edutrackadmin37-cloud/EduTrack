using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ReflectionDAL : BaseDAL
    {
        public int Create(Reflection model)
        {
            SqlParameter outId = new SqlParameter("@NewReflectionID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateReflection",
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@ProjectID", model.ProjectID),
                new SqlParameter("@WeekNumber", model.WeekNumber),
                new SqlParameter("@Content", model.Content),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public List<Reflection> GetByProjectWeek(int projectId, int weekNumber)
        {
            List<Reflection> list = new List<Reflection>();
            using (SqlDataReader r = ExecuteReader("sp_GetReflectionsByProjectWeek",
                new SqlParameter("@ProjectID", projectId),
                new SqlParameter("@WeekNumber", weekNumber)))
            {
                while (r.Read())
                {
                    list.Add(new Reflection
                    {
                        ReflectionID = GetValue<int>(r, "ReflectionID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        ProjectID = GetValue<int>(r, "ProjectID"),
                        WeekNumber = GetValue<int>(r, "WeekNumber"),
                        Content = GetValue<string>(r, "Content"),
                        SubmittedAt = GetValue<DateTime>(r, "SubmittedAt"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName")
                    });
                }
            }
            return list;
        }
    }
}