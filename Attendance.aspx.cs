using EduTrack.BLL;
using EduTrack.Models;
using EduTrack.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace EduTrack.Parent
{
    // Inner DTO for attendance records
    public class AttendanceRecord
    {
        public DateTime AttendanceDate { get; set; }
        public string ClassName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public partial class Attendance : System.Web.UI.Page
    {
        private readonly ParentBLL _parentBLL = new ParentBLL();
        private readonly AttendanceDAL _attendanceDAL = new AttendanceDAL();
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();
        private readonly ClassDAL _classDAL = new ClassDAL();
        private readonly UserDAL _userDAL = new UserDAL();

        private int ParentId => ((User)Session["User"])?.UserID ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Parent")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadChildren();
                txtFromDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                if (ddlChild.Items.Count > 1) ddlChild.SelectedIndex = 1;
                LoadAttendance();
            }
        }

        private void LoadChildren()
        {
            var children = _parentBLL.GetChildren(ParentId);
            ddlChild.DataSource = children.IsSuccess ? children.Data : null;
            ddlChild.DataBind();
            ddlChild.Items.Insert(0, new ListItem("-- Select Child --", "0"));
        }

        protected void ddlChild_SelectedIndexChanged(object sender, EventArgs e) => LoadAttendance();
        protected void btnFilter_Click(object sender, EventArgs e) => LoadAttendance();

        private void LoadAttendance()
        {
            int childId = int.Parse(ddlChild.SelectedValue);
            if (childId == 0)
            {
                pnlSummary.Visible = false;
                gvAttendance.DataSource = null;
                gvAttendance.DataBind();
                return;
            }

            DateTime fromDate = DateTime.Parse(txtFromDate.Text);
            DateTime toDate = DateTime.Parse(txtToDate.Text);

            // Get attendance records for this child
            var records = GetChildAttendance(childId, fromDate, toDate);
            gvAttendance.DataSource = records;
            gvAttendance.DataBind();

            var summary = GetAttendanceSummary(childId, fromDate, toDate);
            if (summary != null)
            {
                pnlSummary.Visible = true;
                lblAttendanceRate.Text = summary.AttendanceRate.ToString("F0") + "%";
                lblPresent.Text = summary.Present.ToString();
                lblAbsent.Text = summary.Absent.ToString();
                lblLate.Text = summary.Late.ToString();
            }
            else
            {
                pnlSummary.Visible = false;
            }
        }

        // ---- Helper methods (replace with BLL calls when available) ----

        private List<AttendanceRecord> GetChildAttendance(int childId, DateTime fromDate, DateTime toDate)
        {
            // Find ClassStudentID for this child
            var classStudents = _classStudentDAL.GetByStudent(childId);
            var cs = classStudents.FirstOrDefault(c => c.IsActive && !c.IsDeleted);
            if (cs == null) return new List<AttendanceRecord>();

            var allAttendance = _attendanceDAL.GetAttendanceByClassStudent(cs.ClassStudentID);
            var filtered = allAttendance
                .Where(a => a.AttendanceDate >= fromDate && a.AttendanceDate <= toDate && !a.IsDeleted)
                .ToList();

            var records = new List<AttendanceRecord>();
            foreach (var att in filtered)
            {
                var cls = _classDAL.GetById(cs.ClassID);
                records.Add(new AttendanceRecord
                {
                    AttendanceDate = att.AttendanceDate,
                    ClassName = cls?.ClassName ?? "N/A",
                    Status = att.Status,
                    Remarks = att.Remarks
                });
            }
            return records.OrderByDescending(r => r.AttendanceDate).ToList();
        }

        private AttendanceSummary GetAttendanceSummary(int childId, DateTime fromDate, DateTime toDate)
        {
            var records = GetChildAttendance(childId, fromDate, toDate);
            if (!records.Any()) return null;

            var present = records.Count(r => r.Status == "Present");
            var absent = records.Count(r => r.Status == "Absent");
            var late = records.Count(r => r.Status == "Late");
            var total = records.Count;
            var rate = total > 0 ? (decimal)present / total * 100 : 0;

            return new AttendanceSummary
            {
                AttendanceRate = rate,
                Present = present,
                Absent = absent,
                Late = late
            };
        }

        // Inner DTO for summary
        public class AttendanceSummary
        {
            public decimal AttendanceRate { get; set; }
            public int Present { get; set; }
            public int Absent { get; set; }
            public int Late { get; set; }
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}