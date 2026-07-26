using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ParentStudentMapBLL
    {
        private readonly ParentStudentMapDAL _dal = new ParentStudentMapDAL();

        public Response<List<ParentStudentMap>> GetChildren(int parentId)
        {
            if (parentId <= 0) return Response<List<ParentStudentMap>>.Failure("Invalid parent ID.", "VALIDATION_ERROR");
            return Response<List<ParentStudentMap>>.Success(_dal.GetChildrenForParent(parentId));
        }

        public Response<int> AddMapping(int parentId, int studentId)
        {
            if (parentId <= 0 || studentId <= 0) return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            int id = _dal.Create(parentId, studentId);
            return id > 0 ? Response<int>.Success(id, "Mapping created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }
    }
}