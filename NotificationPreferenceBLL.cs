// ============================================================
// BLL/NotificationPreferenceBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;

namespace EduTrack.BLL
{
    public class NotificationPreferenceBLL
    {
        private readonly NotificationPreferenceDAL _dal = new NotificationPreferenceDAL();

        public Response<NotificationPreference> GetByUser(int userId)
        {
            if (userId <= 0) return Response<NotificationPreference>.Failure("Invalid User ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByUser(userId);
                return data == null
                    ? Response<NotificationPreference>.Failure("Preferences not found.", "NOT_FOUND")
                    : Response<NotificationPreference>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<NotificationPreference>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(NotificationPreference model)
        {
            if (model == null || model.UserID <= 0)
                return Response<int>.Failure("Invalid preference data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Preferences saved.")
                    : Response<int>.Failure("Save failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(NotificationPreference model)
        {
            if (model == null || model.PreferenceID <= 0)
                return Response<bool>.Failure("Invalid preference data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Preferences updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Preference ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Preferences deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}