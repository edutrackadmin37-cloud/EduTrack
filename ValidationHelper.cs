using System;
using System.Text.RegularExpressions;

namespace EduTrack.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8) return false;
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?\"":{}|<>]");
            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        public static bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && Regex.IsMatch(phone, @"^[0-9\s\-\+]{8,20}$");
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[A-Za-z\s]{2,100}$");
        }

        public static bool IsValidGrade(decimal grade)
        {
            return grade >= 0 && grade <= 100;
        }

        public static bool IsValidDate(DateTime date)
        {
            return date >= new DateTime(2000, 1, 1) && date <= DateTime.Now.AddYears(10);
        }
    }
}