using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class ProjectBLL
    {
        private readonly ProjectDAL _projectDAL = new ProjectDAL();
        private readonly ProjectStatusHistoryDAL _projectStatusHistoryDAL = new ProjectStatusHistoryDAL();

        public Response<List<Project>> GetAllProjects() => Response<List<Project>>.Success(_projectDAL.GetAll());

        public Response<List<Project>> GetProjectsByClassSubjectTeacher(int classSubjectTeacherId)
        {
            if (classSubjectTeacherId <= 0) return Response<List<Project>>.Failure("Invalid class-subject-teacher ID.", "VALIDATION_ERROR");
            return Response<List<Project>>.Success(_projectDAL.GetByClassSubjectTeacher(classSubjectTeacherId));
        }

        public Response<Project> GetProjectById(int projectId)
        {
            if (projectId <= 0) return Response<Project>.Failure("Invalid project ID.", "VALIDATION_ERROR");
            Project item = _projectDAL.GetById(projectId);
            return item == null ? Response<Project>.Failure("Project not found.", "NOT_FOUND") : Response<Project>.Success(item);
        }
        public Response<List<Project>> GetProjectsByTeacher(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<Project>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var allProjects = _projectDAL.GetAll();
                var projects = allProjects.Where(p => p.TeacherID == teacherId).ToList();
                return Response<List<Project>>.Success(projects);
            }
            catch (Exception ex)
            {
                return Response<List<Project>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
        public Response<List<ProjectApprovalDto>> GetPendingApprovals(int headmasterId)
        {
            // reuses the same logic from ReportBLL or call ReportBLL
            var reportBLL = new ReportBLL();
            return reportBLL.GetPendingApprovals(headmasterId);
        }
        public Response<int> CreateProject(Project project)
        {
            if (project == null || project.ClassSubjectTeacherID <= 0 || string.IsNullOrWhiteSpace(project.Title) || project.CreatedBy <= 0)
                return Response<int>.Failure("Invalid project data.", "VALIDATION_ERROR");

            if (project.MaxTeamSize <= 0) project.MaxTeamSize = 5;
            if (string.IsNullOrWhiteSpace(project.Status)) project.Status = "Draft";

            int id = _projectDAL.Create(project);
            return id > 0 ? Response<int>.Success(id, "Project created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateProject(Project project)
        {
            if (project == null || project.ProjectID <= 0) return Response<bool>.Failure("Invalid project data.", "VALIDATION_ERROR");
            bool ok = _projectDAL.Update(project);
            return ok ? Response<bool>.Success(true, "Project updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> MoveProjectStatus(int projectId, string newStatus, int changedBy, string comments)
        {
            if (projectId <= 0 || changedBy <= 0 || string.IsNullOrWhiteSpace(newStatus))
                return Response<bool>.Failure("Invalid status update data.", "VALIDATION_ERROR");

            bool ok = _projectDAL.UpdateStatus(projectId, newStatus, changedBy, comments);
            return ok ? Response<bool>.Success(true, "Project status updated.") : Response<bool>.Failure("Status update failed.", "UPDATE_FAILED");
        }

        public Response<List<ProjectStatusHistory>> GetStatusHistory(int projectId)
        {
            if (projectId <= 0) return Response<List<ProjectStatusHistory>>.Failure("Invalid project ID.", "VALIDATION_ERROR");
            return Response<List<ProjectStatusHistory>>.Success(_projectStatusHistoryDAL.GetByProject(projectId));
        }

        public Response<bool> SoftDeleteProject(int projectId)
        {
            if (projectId <= 0) return Response<bool>.Failure("Invalid project ID.", "VALIDATION_ERROR");
            bool ok = _projectDAL.SoftDelete(projectId);
            return ok ? Response<bool>.Success(true, "Project deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }      
    }
}