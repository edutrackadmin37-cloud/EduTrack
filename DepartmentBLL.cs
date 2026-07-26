using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class DepartmentBLL
    {
        private readonly DepartmentDAL _dal = new DepartmentDAL();

        public Response<List<Department>> GetAll()
        {
            return Response<List<Department>>.Success(_dal.GetAll());
        }

        public Response<Department> GetById(int id)
        {
            if (id <= 0) return Response<Department>.Failure("Invalid Department ID.", "VALIDATION_ERROR");
            Department item = _dal.GetById(id);
            return item == null ? Response<Department>.Failure("Department not found.", "NOT_FOUND") : Response<Department>.Success(item);
        }

        public Response<int> Create(Department item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.DepartmentName))
                return Response<int>.Failure("Department name is required.", "VALIDATION_ERROR");

            int id = _dal.Create(item);
            return id > 0 ? Response<int>.Success(id, "Department created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> Update(Department item)
        {
            if (item == null || item.DepartmentID <= 0)
                return Response<bool>.Failure("Invalid department data.", "VALIDATION_ERROR");

            bool ok = _dal.Update(item);
            return ok ? Response<bool>.Success(true, "Department updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Department ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDelete(id);
            return ok ? Response<bool>.Success(true, "Department deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}