using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TeamDAL : BaseDAL
    {
        public List<Team> GetAll()
        {
            var list = new List<Team>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllProjectTeams"))
            {
                while (r.Read())
                {
                    list.Add(new Team
                    {
                        TeamID = GetValue<int>(r, "TeamID"),
                        ProjectID = GetValue<int>(r, "ProjectID"),
                        TeamName = GetValue<string>(r, "TeamName"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public Team GetById(int teamId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetProjectTeamById", new SqlParameter("@TeamID", teamId)))
            {
                if (r.Read())
                {
                    return new Team
                    {
                        TeamID = GetValue<int>(r, "TeamID"),
                        ProjectID = GetValue<int>(r, "ProjectID"),
                        TeamName = GetValue<string>(r, "TeamName"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    };
                }
                return null;
            }
        }

        public List<Team> GetByProject(int projectId)
        {
            var list = new List<Team>();
            using (SqlDataReader r = ExecuteReader("sp_GetProjectTeamsByProject", new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(new Team
                    {
                        TeamID = GetValue<int>(r, "TeamID"),
                        ProjectID = GetValue<int>(r, "ProjectID"),
                        TeamName = GetValue<string>(r, "TeamName"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public int Create(Team team)
        {
            SqlParameter outId = new SqlParameter("@NewTeamID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateProjectTeam",
                new SqlParameter("@ProjectID", team.ProjectID),
                new SqlParameter("@TeamName", team.TeamName),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Team team)
        {
            return ExecuteNonQuery("sp_UpdateProjectTeam",
                new SqlParameter("@TeamID", team.TeamID),
                new SqlParameter("@TeamName", team.TeamName)) > 0;
        }

        public bool SoftDelete(int teamId)
        {
            return ExecuteNonQuery("sp_SoftDeleteProjectTeam", new SqlParameter("@TeamID", teamId)) > 0;
        }
    }
}