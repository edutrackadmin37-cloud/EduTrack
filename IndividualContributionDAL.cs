using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class IndividualContributionDAL : BaseDAL
    {
        public List<IndividualContribution> GetByTeamAssessment(int teamAssessmentId)
        {
            List<IndividualContribution> list = new List<IndividualContribution>();
            using (SqlDataReader r = ExecuteReader("sp_GetIndividualContributionsByTeamAssessment", new SqlParameter("@TeamAssessmentID", teamAssessmentId)))
            {
                while (r.Read())
                {
                    list.Add(new IndividualContribution
                    {
                        IndividualContributionID = GetValue<int>(r, "IndividualContributionID"),
                        TeamAssessmentID = GetValue<int>(r, "TeamAssessmentID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        IndividualScore = GetValue<decimal>(r, "IndividualScore"),
                        Feedback = GetValue<string>(r, "Feedback"),
                        AssessedBy = GetValue<int>(r, "AssessedBy"),
                        AssessedAt = GetValue<DateTime>(r, "AssessedAt"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName")
                    });
                }
            }
            return list;
        }

        public int Create(IndividualContribution model)
        {
            SqlParameter outId = new SqlParameter("@NewIndividualContributionID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateIndividualContribution",
                new SqlParameter("@TeamAssessmentID", model.TeamAssessmentID),
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@IndividualScore", model.IndividualScore),
                new SqlParameter("@Feedback", (object)model.Feedback ?? DBNull.Value),
                new SqlParameter("@AssessedBy", model.AssessedBy),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(IndividualContribution model)
        {
            int rows = ExecuteNonQuery("sp_UpdateIndividualContribution",
                new SqlParameter("@IndividualContributionID", model.IndividualContributionID),
                new SqlParameter("@IndividualScore", model.IndividualScore),
                new SqlParameter("@Feedback", (object)model.Feedback ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int individualContributionId)
        {
            return ExecuteNonQuery("sp_SoftDeleteIndividualContribution", new SqlParameter("@IndividualContributionID", individualContributionId)) > 0;
        }
    }
}