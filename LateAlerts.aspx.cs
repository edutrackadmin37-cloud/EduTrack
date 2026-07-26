using EduTrack.BLL;
using EduTrack.Models;
using EduTrack.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace EduTrack.Admin
{
    public partial class LateAlerts : System.Web.UI.Page
    {
        private readonly AttendanceBLL _attBLL = new AttendanceBLL();
        private readonly UserBLL _userBLL = new UserBLL();
        private readonly ParentBLL _parentBLL = new ParentBLL();
        private readonly IEmailService _emailService;
        private readonly string _platformUrl;

        public LateAlerts()
        {
            _platformUrl = ConfigurationManager.AppSettings["PlatformURL"] ?? "http://localhost/EduTrack";
            string emailProvider = ConfigurationManager.AppSettings["EmailProvider"]?.ToLower() ?? "smtp";
            if (emailProvider == "sendgrid")
                _emailService = new SendGridEmailService();
            else
                _emailService = new EmailService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSendAlerts_Click(object sender, EventArgs e)
        {
            int threshold = int.TryParse(txtThreshold.Text, out int t) ? t : 3;
            DateTime start = DateTime.Parse(txtStartDate.Text);
            DateTime end = DateTime.Parse(txtEndDate.Text);

            // Get all students who have been late more than threshold in the period
            var allUsers = _userBLL.GetAllUsers();
            if (!allUsers.IsSuccess || allUsers.Data == null)
            {
                ShowToast("Failed to retrieve users.", "error");
                return;
            }

            var students = allUsers.Data.Where(u => u.Role == "Student" && u.IsActive).ToList();
            int sent = 0;
            foreach (var student in students)
            {
                // Get attendance records for this student in the period
                // We need ClassStudentID via ClassStudentDAL
                var classStudentDAL = new DAL.ClassStudentDAL();
                var enrollments = classStudentDAL.GetByStudent(student.UserID);
                var activeEnrollment = enrollments.FirstOrDefault(enrollment => enrollment.IsActive && !enrollment.IsDeleted);
                if (activeEnrollment == null) continue;

                var attendance = _attBLL.GetAttendanceByClassStudent(activeEnrollment.ClassStudentID);
                if (!attendance.IsSuccess || attendance.Data == null) continue;

                int lateCount = attendance.Data.Count(a => a.Status == "Late" && a.AttendanceDate >= start && a.AttendanceDate <= end);
                if (lateCount >= threshold)
                {
                    // Get parent(s) for this student
                    var parents = _parentBLL.GetChildren(student.UserID);
                    if (!parents.IsSuccess || parents.Data == null) continue;
                    foreach (var parent in parents.Data)
                    {
                        // Send email to parent
                        string subject = "EduTrack: Late Arrival Alert for " + student.FullName;
                        string body = $@"
                            <p>Dear Parent,</p>
                            <p>Your child <strong>{student.FullName}</strong> has been marked <strong>Late</strong> {lateCount} times between {start:yyyy-MM-dd} and {end:yyyy-MM-dd}.</p>
                            <p>Please encourage punctuality. You can view attendance details in your <a href='{_platformUrl}/Parent/Dashboard.aspx'>Parent Dashboard</a>.</p>
                            <p>Regards,<br/>EduTrack Team</p>
                        ";
                        var emailResult = _emailService.SendEmailAsync(parent.Email, subject, body);
                        if (emailResult.Result) sent++;
                    }
                }
            }

            ShowToast($"Alerts sent to {sent} parent(s).", "success");
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}