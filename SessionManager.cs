using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace EduTrack.Helpers
{
    public static class SessionManager
    {
        public static void LoginUser(User user, bool rememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                user.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(60),
                rememberMe,
                user.Role
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = false,
                Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(60)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);

            HttpContext.Current.User = new RolePrincipal(new GenericIdentity(user.Email), new[] { user.Role });
            HttpContext.Current.Session["UserID"] = user.UserID;
            HttpContext.Current.Session["Email"] = user.Email;
            HttpContext.Current.Session["FullName"] = user.FullName;
            HttpContext.Current.Session["Role"] = user.Role;
        }

        public static void LogoutUser()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }

        public static User GetCurrentUser()
        {
            if (HttpContext.Current.Session["UserID"] == null) return null;
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserBLL bll = new UserBLL();
            Response<User> response = bll.GetUserById(userId);
            return response.IsSuccess ? response.Data : null;
        }

        public static bool IsInRole(string role)
        {
            return HttpContext.Current.User != null && HttpContext.Current.User.IsInRole(role);
        }
    }
}