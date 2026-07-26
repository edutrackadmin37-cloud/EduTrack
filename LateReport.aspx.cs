using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTrack.Admin
{
    public partial class LateReport : System.Web.UI.Page
    {
        private readonly AttendanceBLL _attBLL = new AttendanceBLL();
        private readonly UserBLL _userBLL = new UserBLL();
        private readonly ClassBLL _classBLL = new ClassBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtFrom.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Parse(txtFrom.Text);
            DateTime to = DateTime.Parse(txtTo.Text);
            int minLates = int.Parse(txtMinLates.Text);

            var reportData = GetLateReport(from, to, minLates);
            gvReport.DataSource = reportData;
            gvReport.DataBind();
        }

        private List<LateReportItem> GetLateReport(DateTime from, DateTime to, int minLates)
        {
            var result = new List<LateReportItem>();
            var students = _userBLL.GetAllUsers();
            if (!students.IsSuccess || students.Data == null) return result;

            var studentList = students.Data.Where(u => u.Role == "Student").ToList();
            foreach (var student in studentList)
            {
                var classStudentDAL = new DAL.ClassStudentDAL();
                var enrollments = classStudentDAL.GetByStudent(student.UserID);
                var active = enrollments.FirstOrDefault(e => e.IsActive && !e.IsDeleted);
                if (active == null) continue;

                var attendance = _attBLL.GetAttendanceByClassStudent(active.ClassStudentID);
                if (!attendance.IsSuccess || attendance.Data == null) continue;

                var inRange = attendance.Data.Where(a => a.AttendanceDate >= from && a.AttendanceDate <= to).ToList();
                int lateCount = inRange.Count(a => a.Status == "Late");
                int total = inRange.Count;
                if (lateCount >= minLates && total > 0)
                {
                    var cls = _classBLL.GetClasses().Data?.FirstOrDefault(c => c.ClassID == active.ClassID);
                    result.Add(new LateReportItem
                    {
                        StudentName = student.FullName,
                        ClassName = cls?.ClassName ?? "N/A",
                        LateCount = lateCount,
                        TotalDays = total,
                        LatePercentage = (decimal)lateCount / total * 100
                    });
                }
            }
            return result.OrderByDescending(r => r.LatePercentage).ToList();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var data = gvReport.DataSource as List<LateReportItem>;
            if (data == null || !data.Any())
            {
                ShowToast("No data to export.", "warning");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Student,Class,Late Days,Total Days,Late %");
            foreach (var item in data)
            {
                sb.AppendLine($"{item.StudentName},{item.ClassName},{item.LateCount},{item.TotalDays},{item.LatePercentage:F1}");
            }

            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename=LateReport_{DateTime.Now:yyyyMMdd}.csv");
            Response.ContentType = "text/csv";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        public class LateReportItem
        {
            public string StudentName { get; set; }
            public string ClassName { get; set; }
            public int LateCount { get; set; }
            public int TotalDays { get; set; }
            public decimal LatePercentage { get; set; }
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}