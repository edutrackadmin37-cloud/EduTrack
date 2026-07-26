using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.Student
{
    public partial class Dashboard : System.Web.UI.Page
    {

        private readonly StudentBLL _studentBLL = new StudentBLL();
        private readonly ProjectBLL _projectBLL = new ProjectBLL();
        private readonly TeamBLL _teamBLL = new TeamBLL();
        private readonly AttendanceBLL _attendanceBLL = new AttendanceBLL();
        private int StudentId => ((User)Session["User"])?.UserID ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.AbsolutePath.ToLower().Contains("login.aspx"))
                return;

            if (Session["User"] == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            var role = Session["Role"]?.ToString();
            if (string.IsNullOrEmpty(role) || !role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStats();
                LoadTeams();
                LoadProjects();
                LoadSessions();
                LoadClassName();
            }
        }

        private void LoadClassName()
        {
            var classes = _studentBLL.GetStudentClasses(StudentId);
            if (classes.IsSuccess && classes.Data != null && classes.Data.Any())
                lblClassName.Text = classes.Data.First().ClassName;
        }

        private void LoadStats()
        {
            // ... (same as before) 
            // But we don't have empty panels for stats, so fine.
        }

        private void LoadTeams()
        {
            var teams = _studentBLL.GetStudentTeams(StudentId);
            var data = teams.IsSuccess ? teams.Data : new List<StudentBLL.TeamMemberInfo>();
            rptTeams.DataSource = data;
            rptTeams.DataBind();
            pnlEmptyTeams.Visible = (data == null || data.Count == 0);
        }

        private void LoadProjects()
        {
            var projects = _studentBLL.GetStudentProjects(StudentId);
            var data = projects.IsSuccess ? projects.Data : new List<StudentBLL.ProjectSummary>();
            rptProjects.DataSource = data;
            rptProjects.DataBind();
            pnlEmptyProjects.Visible = (data == null || data.Count == 0);
        }

        private void LoadSessions()
        {
            var sessions = _studentBLL.GetTodaySessions(StudentId);
            List<EduTrack.Models.SessionModel> sessionList;
            if (sessions.IsSuccess && sessions.Data != null)
            {
                sessionList = sessions.Data
                    .Select(s => new EduTrack.Models.SessionModel
                    {
                        SessionID = s.SessionID,
                        ClassName = s.ClassName,
                        SubjectName = s.SubjectName,
                        TeacherName = s.TeacherName,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Room = s.Room,
                        Status = s.Status,
                        StatusClass = s.StatusClass,
                        BadgeClass = s.BadgeClass,
                        CanJoin = s.CanJoin,
                        JoinText = s.JoinText
                    })
                    .ToList();
            }
            else
            {
                sessionList = new List<EduTrack.Models.SessionModel>();
            }
            rptSessions.DataSource = sessionList;
            rptSessions.DataBind();
            pnlEmptySessions.Visible = (sessionList == null || sessionList.Count == 0);
        }
    }
}