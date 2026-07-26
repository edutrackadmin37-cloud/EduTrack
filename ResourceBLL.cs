using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ResourceBLL
    {
        private readonly ResourceDAL _dal = new ResourceDAL();

        public Response<List<Resource>> GetAllResources()
        {
            return Response<List<Resource>>.Success(_dal.GetAllResources());
        }

        public Response<Resource> GetResourceById(int id)
        {
            if (id <= 0) return Response<Resource>.Failure("Invalid resource ID.", "VALIDATION_ERROR");
            Resource item = _dal.GetResourceById(id);
            return item == null ? Response<Resource>.Failure("Resource not found.", "NOT_FOUND") : Response<Resource>.Success(item);
        }

        public Response<int> CreateResource(Resource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.ResourceTitle) || resource.UploadedBy <= 0)
                return Response<int>.Failure("Invalid resource data.", "VALIDATION_ERROR");

            int id = _dal.CreateResource(resource);
            return id > 0 ? Response<int>.Success(id, "Resource created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateResource(Resource resource)
        {
            if (resource == null || resource.ResourceID <= 0)
                return Response<bool>.Failure("Invalid resource data.", "VALIDATION_ERROR");

            bool ok = _dal.UpdateResource(resource);
            return ok ? Response<bool>.Success(true, "Resource updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDeleteResource(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid resource ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDeleteResource(id);
            return ok ? Response<bool>.Success(true, "Resource deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}