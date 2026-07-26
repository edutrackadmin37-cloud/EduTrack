using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ProjectStatusHistoryDAL : BaseDAL
    {
        public List<ProjectStatusHistory> GetByProject(int projectId)
        {
            List<ProjectStatusHistory> list = new List<ProjectStatusHistory>();
            using (SqlDataReader r = ExecuteReader("sp_GetProjectStatusHistory", new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(new ProjectStatusHistory
                    {
                        ProjectStatusHistoryID = GetValue<int>(r, "ProjectStatusHistoryID"),
                        ProjectID = GetValue<int>(r, "ProjectID"),
                        Status = GetValue<string>(r, "Status"),
                        ChangedBy = GetValue<int>(r, "ChangedBy"),
                        Comments = GetValue<string>(r, "Comments"),
                        ChangedAt = GetValue<DateTime>(r, "ChangedAt"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }
    }
}