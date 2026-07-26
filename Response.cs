using System.Collections.Generic;

namespace EduTrack.Models
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string ErrorCode { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; }

        public static Response<T> Success(T data, string message = "Operation completed successfully.")
        {
            return new Response<T> { IsSuccess = true, Message = message, Data = data };
        }

        public static Response<T> Failure(string message, string errorCode = null)
        {
            return new Response<T> { IsSuccess = false, Message = message, ErrorCode = errorCode };
        }

        public static Response<T> ValidationFailure(Dictionary<string, string> errors)
        {
            return new Response<T> { IsSuccess = false, Message = "Validation failed.", ValidationErrors = errors };
        }
    }
}