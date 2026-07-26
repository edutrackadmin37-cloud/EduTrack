// ============================================================
// BLL/AnalyticsBLL.cs - UPDATED
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class AnalyticsBLL
    {
        private readonly EngagementChecklistDAL _engagementDAL = new EngagementChecklistDAL();
        private readonly ReflectionDAL _reflectionDAL = new ReflectionDAL();
        private readonly AnalyticsDAL _analyticsDAL = new AnalyticsDAL();
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();

        public Response<int> SaveEngagementChecklist(EngagementChecklist item)
        {
            if (item == null || item.ClassStudentID <= 0 || item.ProjectID <= 0 || item.WeekNumber <= 0 || item.MarkedBy <= 0)
                return Response<int>.Failure("Invalid engagement checklist data.", "VALIDATION_ERROR");

            int id = _engagementDAL.Create(item);
            return id > 0 ? Response<int>.Success(id, "Engagement checklist saved.") : Response<int>.Failure("Save failed.", "CREATE_FAILED");
        }

        public Response<List<Reflection>> GetReflectionsByStudentAndProject(int studentId, int projectId)
        {
            if (studentId <= 0 || projectId <= 0)
                return Response<List<Reflection>>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                // Get all reflections for the project and filter by student
                // Since ReflectionDAL doesn't have GetByStudentAndProject, we need to 
                // get all reflections for the project and filter
                var allReflections = _reflectionDAL.GetByProjectWeek(projectId, 0); // Week 0 = all weeks
                // Actually, we need a proper DAL method. For now, we'll use the existing method
                // by getting all weeks from 1 to current week
                var result = new List<Reflection>();
                for (int week = 1; week <= 52; week++)
                {
                    var weekReflections = _reflectionDAL.GetByProjectWeek(projectId, week);
                    result.AddRange(weekReflections.Where(r => r.StudentID == studentId));
                }
                return Response<List<Reflection>>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<List<Reflection>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> SaveReflection(Reflection item)
        {
            if (item == null || item.StudentID <= 0 || item.ProjectID <= 0 || item.WeekNumber <= 0 || string.IsNullOrWhiteSpace(item.Content))
                return Response<int>.Failure("Invalid reflection data.", "VALIDATION_ERROR");

            int id = _reflectionDAL.Create(item);
            return id > 0 ? Response<int>.Success(id, "Reflection saved.") : Response<int>.Failure("Save failed.", "CREATE_FAILED");
        }

        public Response<List<EngagementChecklist>> GetWeeklyEngagement(int projectId, int weekNumber)
        {
            if (projectId <= 0 || weekNumber <= 0) return Response<List<EngagementChecklist>>.Failure("Invalid filters.", "VALIDATION_ERROR");
            return Response<List<EngagementChecklist>>.Success(_engagementDAL.GetByProjectWeek(projectId, weekNumber));
        }

        public Response<List<Reflection>> GetWeeklyReflections(int projectId, int weekNumber)
        {
            if (projectId <= 0 || weekNumber <= 0) return Response<List<Reflection>>.Failure("Invalid filters.", "VALIDATION_ERROR");
            return Response<List<Reflection>>.Success(_reflectionDAL.GetByProjectWeek(projectId, weekNumber));
        }

        public Response<Dictionary<string, decimal>> GetWeeklyEngagementPercentages(int projectId, int weekNumber)
        {
            List<EngagementChecklist> rows = _engagementDAL.GetByProjectWeek(projectId, weekNumber);
            if (rows.Count == 0) return Response<Dictionary<string, decimal>>.Success(new Dictionary<string, decimal>());

            decimal total = rows.Count;

            Dictionary<string, decimal> percentages = new Dictionary<string, decimal>
            {
                { "Participation", rows.Count(x => x.Participation) * 100m / total },
                { "Questioning", rows.Count(x => x.Questioning) * 100m / total },
                { "ProblemSolving", rows.Count(x => x.ProblemSolving) * 100m / total },
                { "Collaboration", rows.Count(x => x.Collaboration) * 100m / total },
                { "TaskCompletion", rows.Count(x => x.TaskCompletion) * 100m / total },
                { "Motivation", rows.Count(x => x.Motivation) * 100m / total }
            };

            return Response<Dictionary<string, decimal>>.Success(percentages);
        }

        #region Subject-Siloed Analytics

        public Response<List<SubjectSiloedPerformance>> GetStudentSubjectPerformance(int studentId, int? subjectId = null, int? academicYearId = null)
        {
            if (studentId <= 0) return Response<List<SubjectSiloedPerformance>>.Failure("Invalid student ID.", "VALIDATION_ERROR");
            try
            {
                var data = _analyticsDAL.GetStudentSubjectPerformance(studentId, subjectId, academicYearId);
                return Response<List<SubjectSiloedPerformance>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SubjectSiloedPerformance>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SubjectSiloedPerformance>> GetClassSubjectPerformance(int classId, int subjectId, int? academicYearId = null)
        {
            if (classId <= 0 || subjectId <= 0) return Response<List<SubjectSiloedPerformance>>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                var data = _analyticsDAL.GetClassSubjectPerformance(classId, subjectId, academicYearId);
                return Response<List<SubjectSiloedPerformance>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SubjectSiloedPerformance>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SubjectSiloedPerformance>> GetTeacherSubjectPerformance(int teacherId, int subjectId, int? academicYearId = null)
        {
            if (teacherId <= 0 || subjectId <= 0) return Response<List<SubjectSiloedPerformance>>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                var data = _analyticsDAL.GetTeacherSubjectPerformance(teacherId, subjectId, academicYearId);
                return Response<List<SubjectSiloedPerformance>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SubjectSiloedPerformance>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        #endregion
    }
}