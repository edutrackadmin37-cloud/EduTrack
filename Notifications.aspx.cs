using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace EduTrack.Parent
{
    public partial class Notifications : System.Web.UI.Page
    {
        private readonly NotificationBLL _notificationBLL = new NotificationBLL();
        private int ParentId => ((User)Session["User"])?.UserID ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Parent")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack) LoadNotifications();
        }

        private void LoadNotifications()
        {
            var notifications = _notificationBLL.GetByUser(ParentId);
            rptNotifications.DataSource = notifications.IsSuccess ? notifications.Data : new List<Notification>();
            rptNotifications.DataBind();

            int unread = 0;
            if (notifications.IsSuccess && notifications.Data != null)
            {
                foreach (var n in notifications.Data)
                    if (!n.IsRead) unread++;
            }
            lblUnread.Text = unread.ToString();
        }

        protected void rptNotifications_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MarkRead")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _notificationBLL.MarkAsRead(id);
                if (result.IsSuccess)
                {
                    ShowToast("Notification marked as read.", "success");
                    LoadNotifications();
                }
            }
            else if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _notificationBLL.SoftDelete(id);
                if (result.IsSuccess)
                {
                    ShowToast("Notification deleted.", "success");
                    LoadNotifications();
                }
            }
        }

        protected void btnMarkAllRead_Click(object sender, EventArgs e)
        {
            var result = _notificationBLL.MarkAllAsRead(ParentId);
            if (result.IsSuccess)
            {
                ShowToast("All notifications marked as read.", "success");
                LoadNotifications();
            }
            else
            {
                ShowToast(result.Message, "error");
            }
        }

        /// <summary>
        /// Returns CSS class for unread notifications
        /// Used in Notifications.aspx to highlight unread items
        /// </summary>
        public string GetUnreadClass(object isReadObj)
        {
            if (isReadObj == null) return "unread";
            try
            {
                bool isRead = Convert.ToBoolean(isReadObj);
                return isRead ? "" : "unread";
            }
            catch
            {
                return "unread";
            }
        }

        /// <summary>
        /// Determines if a notification is unread
        /// Used in Notifications.aspx to control visibility of "Mark Read" button
        /// </summary>
        public bool IsUnread(object isReadObj)
        {
            if (isReadObj == null) return true;
            try
            {
                bool isRead = Convert.ToBoolean(isReadObj);
                return !isRead;
            }
            catch
            {
                return true;
            }
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}
