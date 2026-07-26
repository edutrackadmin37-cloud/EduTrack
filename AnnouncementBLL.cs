using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class AnnouncementBLL
    {
        private readonly AnnouncementDAL _dal = new AnnouncementDAL();

        public Response<List<Announcement>> GetAllAnnouncements()
        {
            return Response<List<Announcement>>.Success(_dal.GetAllAnnouncements());
        }

        public Response<List<Announcement>> GetAnnouncementsByClass(int classId)
        {
            if (classId <= 0) return Response<List<Announcement>>.Failure("Invalid class ID.", "VALIDATION_ERROR");
            return Response<List<Announcement>>.Success(_dal.GetAnnouncementsByClass(classId));
        }

        public Response<Announcement> GetAnnouncementById(int id)
        {
            if (id <= 0) return Response<Announcement>.Failure("Invalid announcement ID.", "VALIDATION_ERROR");
            Announcement item = _dal.GetAnnouncementById(id);
            return item == null ? Response<Announcement>.Failure("Announcement not found.", "NOT_FOUND") : Response<Announcement>.Success(item);
        }

        public Response<int> CreateAnnouncement(Announcement announcement)
        {
            if (announcement == null || string.IsNullOrWhiteSpace(announcement.Title) || announcement.PostedBy <= 0)
                return Response<int>.Failure("Invalid announcement data.", "VALIDATION_ERROR");

            int id = _dal.CreateAnnouncement(announcement);
            return id > 0 ? Response<int>.Success(id, "Announcement created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateAnnouncement(Announcement announcement)
        {
            if (announcement == null || announcement.AnnouncementID <= 0)
                return Response<bool>.Failure("Invalid announcement data.", "VALIDATION_ERROR");

            bool ok = _dal.UpdateAnnouncement(announcement);
            return ok ? Response<bool>.Success(true, "Announcement updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDeleteAnnouncement(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid announcement ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDeleteAnnouncement(id);
            return ok ? Response<bool>.Success(true, "Announcement deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}