using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace EduTrack
{
    public partial class Notifications : System.Web.UI.Page
    {
        private readonly NotificationBLL _notificationBLL = new NotificationBLL();
        private readonly UserBLL _userBLL = new UserBLL();
        private readonly TeacherBLL _teacherBLL = new TeacherBLL();
        private readonly ClassBLL _classBLL = new ClassBLL();
        private readonly ParentStudentMapBLL _parentStudentMapBLL = new ParentStudentMapBLL();

        private int CurrentUserId => Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;
        private string CurrentRole => Session["Role"]?.ToString() ?? "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserId == 0)
            {
                Response.Redirect(ResolveUrl("~/Auth/Login.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                pnlSendNotif.Visible = (CurrentRole == "SystemAdministrator" || CurrentRole == "Teacher");
                LoadNotifications();
                ConfigureTargets();
            }
        }

        private void ConfigureTargets()
        {
            ddlNotifTarget.Items.Clear();

            if (CurrentRole == "SystemAdministrator")
            {
                ddlNotifTarget.Items.Add(new ListItem("All Users", "All"));
                ddlNotifTarget.Items.Add(new ListItem("Teachers", "Teachers"));
                ddlNotifTarget.Items.Add(new ListItem("Students", "Students"));
                ddlNotifTarget.Items.Add(new ListItem("Parents", "Parents"));
            }
            else if (CurrentRole == "Teacher")
            {
                ddlNotifTarget.Items.Add(new ListItem("My Students", "MyStudents"));
                ddlNotifTarget.Items.Add(new ListItem("Parents of My Students", "MyParents"));
                ddlNotifTarget.Items.Add(new ListItem("My Students & Parents", "MyStudentsAndParents"));
            }
        }

        private void LoadNotifications()
        {
            var response = _notificationBLL.GetByUser(CurrentUserId);
            if (response.IsSuccess && response.Data != null)
            {
                var list = response.Data.Select(n => new
                {
                    n.NotificationID,
                    n.NotificationText,
                    n.NotificationDate,
                    ReadStatus = n.IsRead ? "Read" : "Unread"
                }).ToList();

                gvNotifs.DataSource = list;
                gvNotifs.DataBind();

                // Mark all as read
                _notificationBLL.MarkAllAsRead(CurrentUserId);
            }
            else
            {
                gvNotifs.DataSource = null;
                gvNotifs.DataBind();
            }
        }

        private List<int> GetTargetUserIds(string target)
        {
            var userIds = new List<int>();

            if (CurrentRole == "SystemAdministrator")
            {
                var allUsers = _userBLL.GetAllUsers();
                if (!allUsers.IsSuccess || allUsers.Data == null)
                    return userIds;

                var query = allUsers.Data.AsEnumerable();

                if (target == "Teachers")
                    query = query.Where(u => u.Role == "Teacher");
                else if (target == "Students")
                    query = query.Where(u => u.Role == "Student");
                else if (target == "Parents")
                    query = query.Where(u => u.Role == "Parent");
                // "All" -> no filter

                userIds = query.Select(u => u.UserID).ToList();
            }
            else if (CurrentRole == "Teacher")
            {
                var classes = _teacherBLL.GetTeacherClasses(CurrentUserId);
                if (!classes.IsSuccess || classes.Data == null)
                    return userIds;

                var classIds = classes.Data.Select(c => c.ClassID).ToList();

                if (target == "MyStudents")
                {
                    var allStudents = new List<ClassStudent>();
                    foreach (var classId in classIds)
                    {
                        var cs = _classBLL.GetClassStudents(classId);
                        if (cs.IsSuccess && cs.Data != null)
                            allStudents.AddRange(cs.Data);
                    }
                    userIds = allStudents.Select(cs => cs.StudentID).Distinct().ToList();
                }
                else if (target == "MyParents")
                {
                    var studentIds = GetTargetUserIds("MyStudents");
                    foreach (var sid in studentIds)
                    {
                        var children = _parentStudentMapBLL.GetChildren(sid);
                        if (children.IsSuccess && children.Data != null)
                            userIds.AddRange(children.Data.Select(p => p.ParentID));
                    }
                    userIds = userIds.Distinct().ToList();
                }
                else if (target == "MyStudentsAndParents")
                {
                    var students = GetTargetUserIds("MyStudents");
                    var parents = GetTargetUserIds("MyParents");
                    userIds = students.Union(parents).Distinct().ToList();
                }
            }

            return userIds;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string text = txtNotText.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                ShowMessage("Notification text is required.", "warning");
                return;
            }

            var targetUserIds = GetTargetUserIds(ddlNotifTarget.SelectedValue);
            if (!targetUserIds.Any())
            {
                ShowMessage("No valid users found for the selected target.", "warning");
                return;
            }

            int sentCount = 0;
            foreach (var uid in targetUserIds)
            {
                var notif = new Notification
                {
                    UserID = uid,
                    NotificationText = text,
                    NotificationDate = DateTime.Now,
                    IsRead = false
                };
                var result = _notificationBLL.Create(notif);
                if (result.IsSuccess) sentCount++;
            }

            ShowMessage($"Notification sent to {sentCount} user(s).", "success");
            txtNotText.Text = "";
            ddlNotifTarget.SelectedIndex = 0;
            LoadNotifications();
        }

        private void ShowMessage(string message, string type)
        {
            lblNotifMsg.Text = message;
            lblNotifMsg.CssClass = type == "success" ? "alert alert-success" :
                                  type == "warning" ? "alert alert-warning" :
                                  "alert alert-info";
            lblNotifMsg.Visible = true;
        }
    }
}