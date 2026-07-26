using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class MessageBLL
    {
        private readonly MessageDAL _dal = new MessageDAL();

        public Response<List<Message>> GetMessagesByUser(int userId)
        {
            if (userId <= 0) return Response<List<Message>>.Failure("Invalid user ID.", "VALIDATION_ERROR");
            return Response<List<Message>>.Success(_dal.GetByUser(userId));
        }

        public Response<int> SendMessage(Message message)
        {
            if (message == null || message.SenderID <= 0 || message.ReceiverID <= 0)
                return Response<int>.Failure("Invalid message data.", "VALIDATION_ERROR");

            if (string.IsNullOrWhiteSpace(message.Body))
                return Response<int>.Failure("Message body is required.", "VALIDATION_ERROR");

            int id = _dal.Create(message);
            return id > 0 ? Response<int>.Success(id, "Message sent.") : Response<int>.Failure("Send failed.", "CREATE_FAILED");
        }

        public Response<bool> MarkAsRead(int messageId)
        {
            if (messageId <= 0) return Response<bool>.Failure("Invalid message ID.", "VALIDATION_ERROR");
            bool ok = _dal.MarkAsRead(messageId);
            return ok ? Response<bool>.Success(true, "Message marked as read.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDelete(int messageId)
        {
            if (messageId <= 0) return Response<bool>.Failure("Invalid message ID.", "VALIDATION_ERROR");
            bool ok = _dal.SoftDelete(messageId);
            return ok ? Response<bool>.Success(true, "Message deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
    }
}