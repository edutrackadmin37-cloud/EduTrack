using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class StreamBLL
    {
        private readonly StreamDAL _dal = new StreamDAL();

        public Response<List<Stream>> GetAll()
        {
            return Response<List<Stream>>.Success(_dal.GetAll());
        }

        public Response<Stream> GetById(int id)
        {
            if (id <= 0) return Response<Stream>.Failure("Invalid ID.", "VALIDATION_ERROR");
            var item = _dal.GetById(id);
            return item == null ? Response<Stream>.Failure("Not found.", "NOT_FOUND") : Response<Stream>.Success(item);
        }

        public Response<int> Create(Stream model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.StreamName))
                return Response<int>.Failure("Invalid stream data.", "VALIDATION_ERROR");

            int id = _dal.Create(model);
            return id > 0 ? Response<int>.Success(id, "Stream created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> Update(Stream model)
        {
            if (model == null || model.StreamID <= 0)
                return Response<bool>.Failure("Invalid stream data.", "VALIDATION_ERROR");

            bool ok = _dal.Update(model);
            return ok ? Response<bool>.Success(true, "Stream updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDelete(id);
            return ok ? Response<bool>.Success(true, "Stream deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}