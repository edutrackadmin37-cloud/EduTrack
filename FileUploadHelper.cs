using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace EduTrack.Helpers
{
    public static class FileUploadHelper
    {
        private static readonly string[] AllowedExtensions = {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
            ".jpg", ".jpeg", ".png", ".gif", ".zip", ".rar", ".txt",
            ".csv", ".mp4", ".avi", ".mp3", ".wav"
        };

        private const long MaxFileSizeBytes = 25 * 1024 * 1024; // 25MB

        public static bool IsValidExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            return Array.IndexOf(AllowedExtensions, ext) >= 0;
        }

        public static bool IsValidSize(int contentLength)
        {
            return contentLength <= MaxFileSizeBytes;
        }

        public static bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;
            string name = Path.GetFileNameWithoutExtension(fileName);
            return Regex.IsMatch(name, @"^[a-zA-Z0-9\-_\s]+$");
        }

        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return "file";
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            name = Regex.Replace(name, @"[^a-zA-Z0-9\-_]", "");
            return $"{name}_{DateTime.Now.Ticks}{ext}";
        }

        public static string GetSafeFileName(string fileName)
        {
            return SanitizeFileName(fileName);
        }

        public static string SaveFile(HttpPostedFile file, string basePath = "~/App_Data/Uploads/")
        {
            if (file == null || file.ContentLength == 0)
                throw new ArgumentException("No file provided.");

            if (!IsValidExtension(file.FileName))
                throw new ArgumentException($"File type not allowed. Allowed: {string.Join(", ", AllowedExtensions)}");

            if (!IsValidSize(file.ContentLength))
                throw new ArgumentException($"File size exceeds {MaxFileSizeBytes / (1024 * 1024)}MB limit.");

            string physicalDir = HttpContext.Current.Server.MapPath(basePath);
            if (!Directory.Exists(physicalDir))
                Directory.CreateDirectory(physicalDir);

            string safeName = GetSafeFileName(file.FileName);
            string virtualPath = basePath + safeName;
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

            file.SaveAs(physicalPath);
            return virtualPath;
        }

        public static bool DeleteFile(string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath)) return false;
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                return true;
            }
            return false;
        }
    }
}