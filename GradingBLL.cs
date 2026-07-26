using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class GradingBLL
    {
        private readonly TeamAssessmentDAL _teamAssessmentDAL = new TeamAssessmentDAL();
        private readonly IndividualContributionDAL _individualContributionDAL = new IndividualContributionDAL();

        public Response<TeamAssessment> GetTeamAssessmentByTeam(int teamId)
        {
            if (teamId <= 0) return Response<TeamAssessment>.Failure("Invalid team ID.", "VALIDATION_ERROR");
            TeamAssessment item = _teamAssessmentDAL.GetByTeam(teamId);
            return item == null ? Response<TeamAssessment>.Failure("Assessment not found.", "NOT_FOUND") : Response<TeamAssessment>.Success(item);
        }

        public Response<int> CreateTeamAssessment(TeamAssessment model)
        {
            if (model == null || model.TeamID <= 0 || model.RubricID <= 0 || model.AssessedBy <= 0)
                return Response<int>.Failure("Invalid team assessment data.", "VALIDATION_ERROR");

            if (model.TeamScore < 0 || model.TeamScore > 100)
                return Response<int>.Failure("Team score must be between 0 and 100.", "VALIDATION_ERROR");

            int id = _teamAssessmentDAL.Create(model);
            return id > 0 ? Response<int>.Success(id, "Team assessment created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateTeamAssessment(TeamAssessment model)
        {
            if (model == null || model.TeamAssessmentID <= 0) return Response<bool>.Failure("Invalid team assessment data.", "VALIDATION_ERROR");
            bool ok = _teamAssessmentDAL.Update(model);
            return ok ? Response<bool>.Success(true, "Team assessment updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<int> CreateIndividualContribution(IndividualContribution model)
        {
            if (model == null || model.TeamAssessmentID <= 0 || model.StudentID <= 0 || model.AssessedBy <= 0)
                return Response<int>.Failure("Invalid individual contribution data.", "VALIDATION_ERROR");

            if (model.IndividualScore < 0 || model.IndividualScore > 100)
                return Response<int>.Failure("Individual score must be between 0 and 100.", "VALIDATION_ERROR");

            int id = _individualContributionDAL.Create(model);
            return id > 0 ? Response<int>.Success(id, "Individual contribution added.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateIndividualContribution(IndividualContribution model)
        {
            if (model == null || model.IndividualContributionID <= 0) return Response<bool>.Failure("Invalid contribution data.", "VALIDATION_ERROR");
            bool ok = _individualContributionDAL.Update(model);
            return ok ? Response<bool>.Success(true, "Individual contribution updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<List<IndividualContribution>> GetIndividualContributions(int teamAssessmentId)
        {
            if (teamAssessmentId <= 0) return Response<List<IndividualContribution>>.Failure("Invalid team assessment ID.", "VALIDATION_ERROR");
            return Response<List<IndividualContribution>>.Success(_individualContributionDAL.GetByTeamAssessment(teamAssessmentId));
        }

        public Response<Dictionary<string, object>> GetTeamAndIndividualBreakdown(int teamId)
        {
            TeamAssessment ta = _teamAssessmentDAL.GetByTeam(teamId);
            if (ta == null) return Response<Dictionary<string, object>>.Failure("Team assessment not found.", "NOT_FOUND");

            List<IndividualContribution> contributions = _individualContributionDAL.GetByTeamAssessment(ta.TeamAssessmentID);

            Dictionary<string, object> payload = new Dictionary<string, object>
            {
                { "TeamAssessmentID", ta.TeamAssessmentID },
                { "TeamID", ta.TeamID },
                { "TeamScore", ta.TeamScore },
                { "IndividualContributions", contributions },
                { "AverageIndividualScore", contributions.Count == 0 ? 0 : contributions.Average(x => x.IndividualScore) }
            };

            return Response<Dictionary<string, object>>.Success(payload);
        }
    }
}