using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;

namespace EduTrack
{
    public partial class SiteMaster : MasterPage
    {
        private User _currentUser;

        public class MenuItem
        {
            public string Text { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public string ToolTip { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentUser = SessionManager.GetCurrentUser();

            if (_currentUser != null && _currentUser.IsActive && _currentUser.IsApproved)
            {
                // Authenticated user: show user menu, hide anonymous menu
                phAnonNav.Visible = false;
                phUserNav.Visible = true;

                lblUserMenuText.Text = !string.IsNullOrEmpty(_currentUser.FullName)
                    ? _currentUser.FullName.Split(' ')[0] // First name only
                    : "User";

                // Load profile picture
                LoadProfilePicture();

                // Build role-based sidebar
                BuildSidebar(_currentUser.Role);
            }
            else
            {
                // Anonymous user: show login/register, hide user menu
                phAnonNav.Visible = true;
                phUserNav.Visible = false;

                // Hide sidebar for anonymous users
                sidebar.Visible = false;
                mainContentArea.Attributes["class"] = "col-12 main-content";
            }
        }

        /// <summary>
        /// Load and display user's profile picture
        /// </summary>
        private void LoadProfilePicture()
        {
            try
            {
                string pic = _currentUser?.ProfilePicture;

                if (!string.IsNullOrEmpty(pic))
                {
                    string physicalPath = Server.MapPath(pic);
                    if (File.Exists(physicalPath))
                    {
                        imgProfile.ImageUrl = ResolveUrl(pic);
                        return;
                    }
                }

                // Fallback to default avatar
                imgProfile.ImageUrl = ResolveUrl("~/Resources/default-avatar.png");
            }
            catch
            {
                // Default fallback on error
                imgProfile.ImageUrl = ResolveUrl("~/Resources/default-avatar.png");
            }
        }

        /// <summary>
        /// Build sidebar menu based on user role
        /// </summary>
        private void BuildSidebar(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                sidebar.Visible = false;
                return;
            }

            var menuItems = new List<MenuItem>();

            // Add role-specific menu items
            switch (role)
            {
                case "SystemAdministrator":
                    BuildAdminSidebar(menuItems);
                    break;
                case "Teacher":
                    BuildTeacherSidebar(menuItems);
                    break;
                case "Student":
                    BuildStudentSidebar(menuItems);
                    break;
                case "Parent":
                    BuildParentSidebar(menuItems);
                    break;
                case "Headmaster":
                    BuildHeadmasterSidebar(menuItems);
                    break;
                case "AssistantHeadmaster":
                    BuildAssistantHeadmasterSidebar(menuItems);
                    break;
                case "AcademicCoordinator":
                    BuildAcademicCoordinatorSidebar(menuItems);
                    break;
                case "HOD":
                    BuildHODSidebar(menuItems);
                    break;
                default:
                    BuildDefaultSidebar(menuItems);
                    break;
            }

            // Render sidebar HTML
            RenderSidebar(menuItems);
        }

        private void BuildAdminSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/Admin/Dashboard.aspx", ToolTip = "Admin Dashboard" });
            items.Add(new MenuItem { Text = "Users", Icon = "bi-people", Url = "~/Admin/Users.aspx", ToolTip = "Manage Users" });
            items.Add(new MenuItem { Text = "Classes", Icon = "bi-book", Url = "~/Admin/Classes.aspx", ToolTip = "Manage Classes" });
            items.Add(new MenuItem { Text = "Subjects", Icon = "bi-journal", Url = "~/Admin/Subjects.aspx", ToolTip = "Manage Subjects" });
            items.Add(new MenuItem { Text = "Programmes", Icon = "bi-diagram-3", Url = "~/Admin/Programmes.aspx", ToolTip = "Manage Programmes" });
            items.Add(new MenuItem { Text = "Departments", Icon = "bi-building", Url = "~/Admin/Departments.aspx", ToolTip = "Manage Departments" });
            items.Add(new MenuItem { Text = "Academic Years", Icon = "bi-calendar", Url = "~/Admin/AcademicYears.aspx", ToolTip = "Manage Academic Years" });
            items.Add(new MenuItem { Text = "Streams", Icon = "bi-water", Url = "~/Admin/Streams.aspx", ToolTip = "Manage Streams" });
            items.Add(new MenuItem { Text = "Schools", Icon = "bi-house", Url = "~/Admin/Schools.aspx", ToolTip = "Manage Schools" });
            items.Add(new MenuItem { Text = "Staff", Icon = "bi-person-badge", Url = "~/Admin/Staff.aspx", ToolTip = "Manage Staff" });
            items.Add(new MenuItem { Text = "System Settings", Icon = "bi-gear", Url = "~/Admin/SystemSettings.aspx", ToolTip = "System Configuration" });
            items.Add(new MenuItem { Text = "Audit Log", Icon = "bi-journal", Url = "~/Admin/AuditLog.aspx", ToolTip = "View Audit Logs" });
            items.Add(new MenuItem { Text = "Late Alerts", Icon = "bi-envelope-exclamation", Url = "~/Admin/LateAlerts.aspx", ToolTip = "Late Alerts" });
            items.Add(new MenuItem { Text = "Late Report", Icon = "bi-clock-history", Url = "~/Admin/LateReport.aspx", ToolTip = "Late Report" });
        }

        private void BuildTeacherSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/Teacher/Dashboard.aspx", ToolTip = "Teacher Dashboard" });
            items.Add(new MenuItem { Text = "My Classes", Icon = "bi-book", Url = "~/Teacher/MyClasses.aspx", ToolTip = "View Your Classes" });
            items.Add(new MenuItem { Text = "Projects", Icon = "bi-folder", Url = "~/Teacher/Projects.aspx", ToolTip = "Manage Projects" });
            items.Add(new MenuItem { Text = "Teams", Icon = "bi-people", Url = "~/Teacher/Teams.aspx", ToolTip = "View Teams" });
            items.Add(new MenuItem { Text = "Grading", Icon = "bi-star", Url = "~/Teacher/Grading.aspx", ToolTip = "Grade Submissions" });
            items.Add(new MenuItem { Text = "Attendance", Icon = "bi-calendar-check", Url = "~/Teacher/Attendance.aspx", ToolTip = "Mark Attendance" });
            items.Add(new MenuItem { Text = "Rubrics", Icon = "bi-ui-checks", Url = "~/Teacher/Rubrics.aspx", ToolTip = "Manage Rubrics" });
            items.Add(new MenuItem { Text = "Tests", Icon = "bi-clipboard-check", Url = "~/Teacher/Tests.aspx", ToolTip = "Create Tests" });
            items.Add(new MenuItem { Text = "Questions", Icon = "bi-question-circle", Url = "~/Teacher/Questions.aspx", ToolTip = "Manage Questions" });
            items.Add(new MenuItem { Text = "Resources", Icon = "bi-folder", Url = "~/Teacher/Resources.aspx", ToolTip = "Upload Resources" });
            items.Add(new MenuItem { Text = "Submissions", Icon = "bi-file-earmark-check", Url = "~/Teacher/Submissions.aspx", ToolTip = "View Submissions" });
            items.Add(new MenuItem { Text = "Reports", Icon = "bi-file-earmark-text", Url = "~/Teacher/Reports.aspx", ToolTip = "Generate Reports" });
        }

        private void BuildStudentSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/Student/Dashboard.aspx", ToolTip = "Student Dashboard" });
            items.Add(new MenuItem { Text = "My Projects", Icon = "bi-folder", Url = "~/Student/MyProjects.aspx", ToolTip = "View Your Projects" });
            items.Add(new MenuItem { Text = "My Teams", Icon = "bi-people", Url = "~/Student/MyTeams.aspx", ToolTip = "View Your Teams" });
            items.Add(new MenuItem { Text = "Messages", Icon = "bi-chat-dots", Url = "~/Student/Messages.aspx", ToolTip = "Send Messages" });
            items.Add(new MenuItem { Text = "Reflections", Icon = "bi-journal", Url = "~/Student/Reflections.aspx", ToolTip = "Write Reflections" });
            items.Add(new MenuItem { Text = "Grades", Icon = "bi-star", Url = "~/Student/Grades.aspx", ToolTip = "View Your Grades" });
            items.Add(new MenuItem { Text = "Resources", Icon = "bi-folder", Url = "~/Student/Resources.aspx", ToolTip = "View Resources" });
            items.Add(new MenuItem { Text = "Peer Assessments", Icon = "bi-person-rolodex", Url = "~/Student/PeerAssessments.aspx", ToolTip = "Assess Peers" });
            items.Add(new MenuItem { Text = "Take Test", Icon = "bi-pencil-square", Url = "~/Student/TakeTest.aspx", ToolTip = "Take Tests" });
            items.Add(new MenuItem { Text = "Test Results", Icon = "bi-bar-chart-line", Url = "~/Student/TestResults.aspx", ToolTip = "View Results" });
        }

        private void BuildParentSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/Parent/Dashboard.aspx", ToolTip = "Parent Dashboard" });
            items.Add(new MenuItem { Text = "Child Performance", Icon = "bi-person", Url = "~/Parent/ChildPerformance.aspx", ToolTip = "View Child's Performance" });
            items.Add(new MenuItem { Text = "Attendance", Icon = "bi-calendar-check", Url = "~/Parent/Attendance.aspx", ToolTip = "View Attendance" });
            items.Add(new MenuItem { Text = "Reports", Icon = "bi-file-earmark-text", Url = "~/Parent/Reports.aspx", ToolTip = "View Reports" });
            items.Add(new MenuItem { Text = "Notifications", Icon = "bi-bell", Url = "~/Parent/Notifications.aspx", ToolTip = "View Notifications" });
        }

        private void BuildHeadmasterSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/Headmaster/Dashboard.aspx", ToolTip = "Headmaster Dashboard" });
            items.Add(new MenuItem { Text = "School Overview", Icon = "bi-house", Url = "~/Headmaster/SchoolOverview.aspx", ToolTip = "School Overview" });
            items.Add(new MenuItem { Text = "Reports", Icon = "bi-file-earmark-text", Url = "~/Headmaster/Reports.aspx", ToolTip = "Generate Reports" });
        }

        private void BuildAssistantHeadmasterSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/AssistantHeadmaster/Dashboard.aspx", ToolTip = "Dashboard" });
            items.Add(new MenuItem { Text = "Supervision", Icon = "bi-clipboard-check", Url = "~/AssistantHeadmaster/Supervision.aspx", ToolTip = "Supervision Tasks" });
        }

        private void BuildAcademicCoordinatorSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/AcademicCoordinator/Dashboard.aspx", ToolTip = "Dashboard" });
            items.Add(new MenuItem { Text = "Academic Plans", Icon = "bi-calendar", Url = "~/AcademicCoordinator/AcademicPlans.aspx", ToolTip = "Academic Plans" });
        }

        private void BuildHODSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-speedometer2", Url = "~/HOD/Dashboard.aspx", ToolTip = "Dashboard" });
            items.Add(new MenuItem { Text = "Department", Icon = "bi-building", Url = "~/HOD/Department.aspx", ToolTip = "Department Management" });
        }

        private void BuildDefaultSidebar(List<MenuItem> items)
        {
            items.Add(new MenuItem { Text = "Dashboard", Icon = "bi-grid-1x2-fill", Url = "~/Default.aspx", ToolTip = "Dashboard" });
        }

        /// <summary>
        /// Render sidebar menu items as HTML
        /// </summary>
        private void RenderSidebar(List<MenuItem> menuItems)
        {
            StringBuilder sb = new StringBuilder();

            // Sidebar brand/logo
            sb.Append("<div class='sidebar-brand'>");
            sb.Append("<img src='" + ResolveUrl("~/Image/DVT-0185.jpg") + "' class='logo-sidebar' alt='EduTrack Logo' />");
            sb.Append("<span class='brand-text'>EduTrack</span>");
            sb.Append("</div>");

            // Sidebar header
            sb.Append("<div class='sidebar-header'>NAVIGATION</div>");
            sb.Append("<div class='nav flex-column'>");

            // Determine current page
            string currentUrl = Request.AppRelativeCurrentExecutionFilePath.ToLower();

            // Generate menu items
            foreach (var item in menuItems)
            {
                string url = ResolveUrl(item.Url);
                string itemPath = item.Url.Replace("~/", "").ToLower();
                string isActive = currentUrl.Contains(itemPath) ? " active" : "";

                sb.Append("<a href='" + url + "' class='nav-link" + isActive + "' title='" + (item.ToolTip ?? item.Text) + "'>");
                sb.Append("<i class='bi " + item.Icon + "'></i> ");
                sb.Append(item.Text);
                sb.Append("</a>");
            }

            sb.Append("</div>");

            // Clear and render
            phSidebarContent.Controls.Clear();
            phSidebarContent.Controls.Add(new LiteralControl(sb.ToString()));

            // Ensure sidebar is visible
            sidebar.Visible = true;
            mainContentArea.Attributes["class"] = "col-12 main-content with-sidebar";
        }
    }
}