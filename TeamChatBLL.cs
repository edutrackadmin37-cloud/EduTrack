// ============================================================
// BLL/TeamChatBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class TeamChatBLL
    {
        private readonly TeamChatDAL _dal = new TeamChatDAL();

        public Response<List<TeamChatMessage>> GetByTeam(int teamId)
        {
            if (teamId <= 0) return Response<List<TeamChatMessage>>.Failure("Invalid Team ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByTeam(teamId);
                return Response<List<TeamChatMessage>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<TeamChatMessage>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<TeamChatMessage> GetById(int messageId)
        {
            if (messageId <= 0) return Response<TeamChatMessage>.Failure("Invalid Message ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(messageId);
                return data == null
                    ? Response<TeamChatMessage>.Failure("Message not found.", "NOT_FOUND")
                    : Response<TeamChatMessage>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<TeamChatMessage>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> SendMessage(TeamChatMessage model)
        {
            if (model == null || model.TeamID <= 0 || model.SenderID <= 0 || string.IsNullOrWhiteSpace(model.MessageText))
                return Response<int>.Failure("Invalid message data.", "VALIDATION_ERROR");

            if (!string.IsNullOrWhiteSpace(model.FilePath) && model.FilePath.Length > 255)
                return Response<int>.Failure("File path too long.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Message sent.")
                    : Response<int>.Failure("Send failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> MarkAsRead(int messageId)
        {
            if (messageId <= 0) return Response<bool>.Failure("Invalid Message ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.MarkAsRead(messageId);
                return ok
                    ? Response<bool>.Success(true, "Message marked as read.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int messageId)
        {
            if (messageId <= 0) return Response<bool>.Failure("Invalid Message ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(messageId);
                return ok
                    ? Response<bool>.Success(true, "Message deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}