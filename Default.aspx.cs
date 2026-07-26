using System;

namespace EduTrack
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Prevent redirect loop if we are already on the login or register page
            string currentPath = Request.Url.AbsolutePath.ToLower();
            if (currentPath.Contains("login.aspx") || currentPath.Contains("register.aspx"))
                return;

            // If the user is logged in (session exists), redirect to their dashboard
            if (Session["User"] != null)
            {
                string role = Session["Role"]?.ToString() ?? string.Empty;

                // Use case‑insensitive comparison for safety
                switch (role.ToLower())
                {
                    case "systemadministrator":
                        Response.Redirect("~/Admin/Dashboard.aspx");
                        break;
                    case "teacher":
                        Response.Redirect("~/Teacher/Dashboard.aspx");
                        break;
                    case "student":
                        Response.Redirect("~/Student/Dashboard.aspx");
                        break;
                    case "parent":
                        Response.Redirect("~/Parent/Dashboard.aspx");
                        break;
                    case "headmaster":
                        Response.Redirect("~/Headmaster/Dashboard.aspx");
                        break;
                    case "assistantheadmaster":
                        Response.Redirect("~/AssistantHeadmaster/Dashboard.aspx");
                        break;
                    case "academiccoordinator":
                        Response.Redirect("~/AcademicCoordinator/Dashboard.aspx");
                        break;
                    case "hod":
                        Response.Redirect("~/HOD/Dashboard.aspx");
                        break;
                    default:
                        // If role is unknown, send to login
                        Response.Redirect("~/Auth/Login.aspx");
                        break;
                }
            }
            // Otherwise, show the public landing page (no redirect)
        }
    }
}