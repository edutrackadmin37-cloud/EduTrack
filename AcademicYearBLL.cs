using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class AcademicYearBLL
    {
        private readonly AcademicYearDAL _dal = new AcademicYearDAL();

        public Response<List<AcademicYear>> GetAll()
        {
            return Response<List<AcademicYear>>.Success(_dal.GetAll());
        }

        public Response<AcademicYear> GetById(int id)
        {
            if (id <= 0) return Response<AcademicYear>.Failure("Invalid ID.", "VALIDATION_ERROR");
            var item = _dal.GetById(id);
            return item == null ? Response<AcademicYear>.Failure("Not found.", "NOT_FOUND") : Response<AcademicYear>.Success(item);
        }

        public Response<AcademicYear> GetCurrent()
        {
            var item = _dal.GetCurrent();
            return item == null ? Response<AcademicYear>.Failure("No current year.", "NOT_FOUND") : Response<AcademicYear>.Success(item);
        }

        public Response<int> Create(AcademicYear model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.YearName))
                return Response<int>.Failure("Invalid academic year data.", "VALIDATION_ERROR");

            int id = _dal.Create(model);
            return id > 0 ? Response<int>.Success(id, "Academic year created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> Update(AcademicYear model)
        {
            if (model == null || model.AcademicYearID <= 0)
                return Response<bool>.Failure("Invalid academic year data.", "VALIDATION_ERROR");

            bool ok = _dal.Update(model);
            return ok ? Response<bool>.Success(true, "Academic year updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDelete(id);
            return ok ? Response<bool>.Success(true, "Academic year deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}