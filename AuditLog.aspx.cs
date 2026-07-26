using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class AuditLog : System.Web.UI.Page
    {
        private readonly ActivityLogBLL _logBLL = new ActivityLogBLL();
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUsers();
                LoadActions();
                txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadLogs();
            }
        }

        private void LoadUsers()
        {
            var users = _userBLL.GetAllUsers();
            ddlUser.DataSource = users.IsSuccess ? users.Data : null;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- All Users --", "0"));
        }

        private void LoadActions()
        {
            // If you want to show all possible actions, you need to define them here,
            // or retrieve them from your logs if possible.
            var logs = _logBLL.GetActivityLogs();
            var actions = logs.IsSuccess && logs.Data != null
                ? logs.Data.Select(l => l.Action).Distinct().ToList()
                : new List<string>();

            ddlAction.DataSource = actions;
            ddlAction.DataBind();
            ddlAction.Items.Insert(0, new ListItem("-- All Actions --", ""));
        }

        private void LoadLogs()
        {
            int? userId = int.TryParse(ddlUser.SelectedValue, out int u) && u > 0 ? (int?)u : null;
            string action = ddlAction.SelectedValue == "" ? null : ddlAction.SelectedValue;
            DateTime? from = DateTime.TryParse(txtFromDate.Text, out DateTime f) ? (DateTime?)f : null;
            DateTime? to = DateTime.TryParse(txtToDate.Text, out DateTime t) ? (DateTime?)t : null;

            var logs = _logBLL.GetActivityLogs(userId, action, from, to);
            gvLogs.DataSource = logs.IsSuccess ? logs.Data : new List<ActivityLog>();
            gvLogs.DataBind();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvLogs.PageIndex = 0;
            LoadLogs();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var logs = _logBLL.GetActivityLogs(null, null, null, null);
            if (!logs.IsSuccess || logs.Data == null || !logs.Data.Any())
            {
                ShowToast("No data to export.", "warning");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("User,Action,Date/Time,IP,Details");
            foreach (var log in logs.Data)
            {
                sb.AppendLine($"\"{log.FullName}\",\"{log.Action}\",\"{log.ActionDate:yyyy-MM-dd HH:mm}\",\"{log.IPAddress}\",\"{log.Details?.Replace(",", ";")}\"");
            }

            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename=AuditLog_{DateTime.Now:yyyyMMdd}.csv");
            Response.ContentType = "text/csv";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLogs.PageIndex = e.NewPageIndex;
            LoadLogs();
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}