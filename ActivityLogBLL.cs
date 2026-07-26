using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ActivityLogBLL
    {
        private readonly ActivityLogDAL _dal = new ActivityLogDAL();

        public Response<List<ActivityLog>> GetActivityLogs(int? userId = null, string action = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                List<ActivityLog> logs = _dal.GetActivityLogs(userId, action, fromDate, toDate);
                return Response<List<ActivityLog>>.Success(logs);
            }
            catch (Exception ex)
            {
                return Response<List<ActivityLog>>.Failure($"Unable to fetch activity logs: {ex.Message}", "BLL_ERROR");
            }
        }

        public Response<int> CreateActivityLog(ActivityLog log)
        {
            if (log == null || log.UserID <= 0 || string.IsNullOrWhiteSpace(log.Action))
                return Response<int>.Failure("Invalid activity log data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.CreateActivityLog(log);
                return id > 0 ? Response<int>.Success(id, "Activity logged.") : Response<int>.Failure("Logging failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Unable to log activity: {ex.Message}", "BLL_ERROR");
            }
        }

        public Response<List<string>> GetDistinctActions()
        {
            try
            {
                var actions = _dal.GetDistinctActions();
                return Response<List<string>>.Success(actions);
            }
            catch (Exception ex)
            {
                return Response<List<string>>.Failure($"Unable to fetch distinct actions: {ex.Message}", "BLL_ERROR");
            }
        }
    }
}