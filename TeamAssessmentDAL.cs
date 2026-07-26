using EduTrack.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TeamAssessmentDAL : BaseDAL
    {
        public TeamAssessment GetByTeam(int teamId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetTeamAssessmentByTeam", new SqlParameter("@TeamID", teamId)))
            {
                if (!r.Read()) return null;
                return new TeamAssessment
                {
                    TeamAssessmentID = GetValue<int>(r, "TeamAssessmentID"),
                    TeamID = GetValue<int>(r, "TeamID"),
                    RubricID = GetValue<int>(r, "RubricID"),
                    TeamScore = GetValue<decimal>(r, "TeamScore"),
                    Comments = GetValue<string>(r, "Comments"),
                    AssessedBy = GetValue<int>(r, "AssessedBy"),
                    AssessedAt = GetValue<DateTime>(r, "AssessedAt"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(TeamAssessment model)
        {
            SqlParameter outId = new SqlParameter("@NewTeamAssessmentID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateTeamAssessment",
                new SqlParameter("@TeamID", model.TeamID),
                new SqlParameter("@RubricID", model.RubricID),
                new SqlParameter("@TeamScore", model.TeamScore),
                new SqlParameter("@Comments", (object)model.Comments ?? DBNull.Value),
                new SqlParameter("@AssessedBy", model.AssessedBy),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(TeamAssessment model)
        {
            int rows = ExecuteNonQuery("sp_UpdateTeamAssessment",
                new SqlParameter("@TeamAssessmentID", model.TeamAssessmentID),
                new SqlParameter("@RubricID", model.RubricID),
                new SqlParameter("@TeamScore", model.TeamScore),
                new SqlParameter("@Comments", (object)model.Comments ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int teamAssessmentId)
        {
            return ExecuteNonQuery("sp_SoftDeleteTeamAssessment", new SqlParameter("@TeamAssessmentID", teamAssessmentId)) > 0;
        }
    }
}