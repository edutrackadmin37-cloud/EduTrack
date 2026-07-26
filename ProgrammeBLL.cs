using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ProgrammeBLL
    {
        private readonly ProgrammeDAL _dal = new ProgrammeDAL();

        public Response<List<Programme>> GetAll()
        {
            return Response<List<Programme>>.Success(_dal.GetAll());
        }

        public Response<Programme> GetById(int id)
        {
            if (id <= 0) return Response<Programme>.Failure("Invalid Programme ID.", "VALIDATION_ERROR");
            Programme item = _dal.GetById(id);
            return item == null ? Response<Programme>.Failure("Programme not found.", "NOT_FOUND") : Response<Programme>.Success(item);
        }

        public Response<int> Create(Programme item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.ProgrammeName) || item.DepartmentID <= 0)
                return Response<int>.Failure("Invalid programme data.", "VALIDATION_ERROR");

            int id = _dal.Create(item);
            return id > 0 ? Response<int>.Success(id, "Programme created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> Update(Programme item)
        {
            if (item == null || item.ProgrammeID <= 0 || item.DepartmentID <= 0)
                return Response<bool>.Failure("Invalid programme data.", "VALIDATION_ERROR");

            bool ok = _dal.Update(item);
            return ok ? Response<bool>.Success(true, "Programme updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Programme ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDelete(id);
            return ok ? Response<bool>.Success(true, "Programme deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}