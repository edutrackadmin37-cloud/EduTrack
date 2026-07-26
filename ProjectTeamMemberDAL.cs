using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ProjectTeamMemberDAL : BaseDAL
    {
        // NEW: Get all members
        public List<ProjectTeamMember> GetAll()
        {
            var list = new List<ProjectTeamMember>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllProjectTeamMembers"))
            {
                while (r.Read())
                {
                    list.Add(new ProjectTeamMember
                    {
                        TeamMemberID = GetValue<int>(r, "TeamMemberID"),
                        TeamID = GetValue<int>(r, "TeamID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public List<ProjectTeamMember> GetByTeam(int teamId)
        {
            List<ProjectTeamMember> list = new List<ProjectTeamMember>();
            using (SqlDataReader r = ExecuteReader("sp_GetProjectTeamMembers", new SqlParameter("@TeamID", teamId)))
            {
                while (r.Read())
                {
                    list.Add(new ProjectTeamMember
                    {
                        TeamMemberID = GetValue<int>(r, "TeamMemberID"),
                        TeamID = GetValue<int>(r, "TeamID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName"),
                        Email = GetValue<string>(r, "Email")
                    });
                }
            }
            return list;
        }

        public int Add(int teamId, int studentId)
        {
            SqlParameter outId = new SqlParameter("@NewTeamMemberID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_AddProjectTeamMember",
                new SqlParameter("@TeamID", teamId),
                new SqlParameter("@StudentID", studentId),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Remove(int teamMemberId)
        {
            return ExecuteNonQuery("sp_RemoveProjectTeamMember", new SqlParameter("@TeamMemberID", teamMemberId)) > 0;
        }
    }
}