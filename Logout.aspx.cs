using EduTrack.Helpers;
using System;

namespace EduTrack.Auth
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if already logged out
                if (SessionManager.GetCurrentUser() == null)
                {
                    Response.Redirect(ResolveUrl("~/Auth/Login.aspx"));
                    return;
                }

                // Display logout confirmation message
                if (Request.QueryString["loggedout"] == "1")
                {
                    lblMessage.Text = "You have been logged out successfully.";
                    lblMessage.CssClass = "alert alert-success";
                    lblMessage.Visible = true;
                    btnConfirmLogout.Enabled = false;
                }
            }
        }

        protected void btnConfirmLogout_Click(object sender, EventArgs e)
        {
            SessionManager.LogoutUser();
            Response.Redirect(ResolveUrl("~/Auth/Login.aspx?loggedout=1"));
        }
    }
}