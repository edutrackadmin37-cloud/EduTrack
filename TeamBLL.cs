using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class TeamBLL
    {
        private readonly TeamDAL _teamDAL = new TeamDAL();
        private readonly ProjectTeamMemberDAL _projectTeamMemberDAL = new ProjectTeamMemberDAL();
        private readonly ProjectBLL _projectBLL = new ProjectBLL(); // Added this line

        public Response<List<Team>> GetTeamsByProject(int projectId)
        {
            if (projectId <= 0) return Response<List<Team>>.Failure("Invalid project ID.", "VALIDATION_ERROR");
            return Response<List<Team>>.Success(_teamDAL.GetByProject(projectId));
        }

        public Response<int> CreateTeam(Team team)
        {
            if (team == null || team.ProjectID <= 0 || string.IsNullOrWhiteSpace(team.TeamName))
                return Response<int>.Failure("Invalid team data.", "VALIDATION_ERROR");

            int id = _teamDAL.Create(team);
            return id > 0 ? Response<int>.Success(id, "Team created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateTeam(Team team)
        {
            if (team == null || team.TeamID <= 0) return Response<bool>.Failure("Invalid team data.", "VALIDATION_ERROR");
            bool ok = _teamDAL.Update(team);
            return ok ? Response<bool>.Success(true, "Team updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDeleteTeam(int teamId)
        {
            if (teamId <= 0) return Response<bool>.Failure("Invalid team ID.", "VALIDATION_ERROR");
            bool ok = _teamDAL.SoftDelete(teamId);
            return ok ? Response<bool>.Success(true, "Team deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }

        public Response<int> AddTeamMember(int teamId, int studentId)
        {
            if (teamId <= 0 || studentId <= 0) return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            int id = _projectTeamMemberDAL.Add(teamId, studentId);
            if (id == -1) return Response<int>.Failure("Student already in team.", "DUPLICATE");
            return id > 0 ? Response<int>.Success(id, "Team member added.") : Response<int>.Failure("Add failed.", "CREATE_FAILED");
        }

        public Response<bool> RemoveTeamMember(int teamMemberId)
        {
            if (teamMemberId <= 0) return Response<bool>.Failure("Invalid team member ID.", "VALIDATION_ERROR");
            bool ok = _projectTeamMemberDAL.Remove(teamMemberId);
            return ok ? Response<bool>.Success(true, "Team member removed.") : Response<bool>.Failure("Remove failed.", "DELETE_FAILED");
        }

        public Response<List<ProjectTeamMember>> GetTeamMembers(int teamId)
        {
            if (teamId <= 0) return Response<List<ProjectTeamMember>>.Failure("Invalid team ID.", "VALIDATION_ERROR");
            return Response<List<ProjectTeamMember>>.Success(_projectTeamMemberDAL.GetByTeam(teamId));
        }
        public Response<List<Team>> GetTeamsByTeacher(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<Team>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var projects = _projectBLL.GetProjectsByTeacher(teacherId);
                if (!projects.IsSuccess) return Response<List<Team>>.Failure(projects.Message, "PROJECT_ERROR");

                var allTeams = new List<Team>();
                foreach (var project in projects.Data)
                {
                    var teams = _teamDAL.GetByProject(project.ProjectID);
                    allTeams.AddRange(teams);
                }
                return Response<List<Team>>.Success(allTeams);
            }
            catch (Exception ex)
            {
                return Response<List<Team>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}