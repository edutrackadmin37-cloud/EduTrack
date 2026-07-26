using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.Parent
{
    public partial class ChildPerformance : System.Web.UI.Page
    {
        private readonly ParentBLL _parentBLL = new ParentBLL();
        private int ParentId => ((User)Session["User"])?.UserID ?? 0;
        private int ChildId => Request.QueryString["childId"] != null ? int.Parse(Request.QueryString["childId"]) : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Parent")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (ChildId == 0)
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!_parentBLL.IsChildOfParent(ChildId, ParentId).IsSuccess || !_parentBLL.IsChildOfParent(ChildId, ParentId).Data)
            {
                ShowToast("You do not have access to this child's data.", "error");
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadChildInfo();
                LoadSubjectPerformance();
            }
        }

        private void LoadChildInfo()
        {
            var info = _parentBLL.GetChildInfo(ChildId);
            if (info.IsSuccess && info.Data != null)
            {
                lblChildName.Text = info.Data.FullName;
                lblClass.Text = info.Data.ClassName;
                lblAttendance.Text = info.Data.AttendanceRate.ToString("F0") + "%";
                lblOverallAvg.Text = info.Data.OverallAverage.ToString("F1");
            }
        }

        private void LoadSubjectPerformance()
        {
            var subjects = _parentBLL.GetChildSubjectPerformance(ChildId);
            if (subjects.IsSuccess)
            {
                rptSubjects.DataSource = subjects.Data;
            }
            else
            {
                rptSubjects.DataSource = new List<ParentBLL.SubjectPerformance>();
            }
            rptSubjects.DataBind();
        }

        /// <summary>
        /// Converts numeric grade to letter grade (A, B, C, D, F)
        /// </summary>
        public string GetGradeLetter(object gradeObj)
        {
            if (gradeObj == null) return "F";
            try
            {
                decimal grade = Convert.ToDecimal(gradeObj);
                if (grade >= 90) return "A";
                if (grade >= 80) return "B";
                if (grade >= 70) return "C";
                if (grade >= 60) return "D";
                return "F";
            }
            catch
            {
                return "F";
            }
        }

        /// <summary>
        /// Returns Bootstrap progress bar color class based on grade
        /// </summary>
        public string GetProgressClass(object gradeObj)
        {
            if (gradeObj == null) return "danger";
            try
            {
                decimal grade = Convert.ToDecimal(gradeObj);
                if (grade >= 80) return "success";
                if (grade >= 60) return "info";
                if (grade >= 40) return "warning";
                return "danger";
            }
            catch
            {
                return "danger";
            }
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}
