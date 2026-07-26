using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class ReflectionBLL
    {
        private readonly ReflectionDAL _dal = new ReflectionDAL();

        public Response<int> CreateReflection(Reflection reflection)
        {
            if (reflection == null || reflection.StudentID <= 0 || reflection.ProjectID <= 0 || reflection.WeekNumber <= 0 || string.IsNullOrWhiteSpace(reflection.Content))
                return Response<int>.Failure("Invalid reflection data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(reflection);
                return id > 0 ? Response<int>.Success(id, "Reflection saved.") : Response<int>.Failure("Save failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Reflection>> GetStudentReflections(int studentId)
        {
            if (studentId <= 0)
                return Response<List<Reflection>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                // Need to fetch all reflections for this student – there is no direct DAL method, we'll have to get by project and filter
                // For simplicity, we'll get all reflections from all projects (maybe not efficient)
                // Better to add method in DAL, but we'll do a workaround: get projects of student via ProjectTeamMemberDAL
                var memberDAL = new ProjectTeamMemberDAL();
                var memberships = memberDAL.GetAll().Where(m => m.StudentID == studentId && !m.IsDeleted).ToList();
                var teamIds = memberships.Select(m => m.TeamID).Distinct().ToList();
                var teamDAL = new ProjectDAL();
                var projects = teamDAL.GetAll().Where(p => teamIds.Contains(p.ProjectID) && !p.IsDeleted).ToList();
                var projectIds = projects.Select(p => p.ProjectID).Distinct().ToList();

                var reflections = new List<Reflection>();
                foreach (var pid in projectIds)
                {
                    // Get all weeks for this project – we need to get all weeks from 1 to 52
                    for (int week = 1; week <= 52; week++)
                    {
                        var weekRefs = _dal.GetByProjectWeek(pid, week);
                        reflections.AddRange(weekRefs.Where(r => r.StudentID == studentId));
                    }
                }
                return Response<List<Reflection>>.Success(reflections);
            }
            catch (Exception ex)
            {
                return Response<List<Reflection>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}