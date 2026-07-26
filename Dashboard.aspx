<%@ Page Title="Student Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="EduTrack.Student.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* same styles as before */
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }
        .stat-card { background: rgba(255,255,255,0.95); backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,0.3); border-radius: 16px; padding: 1.5rem; box-shadow: 0 8px 32px rgba(0,0,0,0.08); transition: transform 0.3s ease; }
        .stat-card:hover { transform: translateY(-5px); }
        .stat-card .stat-icon { width: 50px; height: 50px; border-radius: 12px; display: flex; align-items: center; justify-content: center; font-size: 1.8rem; background: var(--primary-gradient); color: white; }
        .stat-number { font-size: 2.2rem; font-weight: 700; color: #2d3748; }
        .stat-label { color: #718096; font-weight: 500; }
        .card { border: none; box-shadow: 0 8px 32px rgba(0,0,0,0.08); border-radius: 12px; background: rgba(255,255,255,0.95); backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,0.3); transition: transform 0.3s ease; }
        .card:hover { transform: translateY(-5px); }
        .btn-gradient { background: var(--primary-gradient); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
        .btn-outline-gradient { background: transparent; color: var(--primary-color); border: 2px solid var(--primary-color); border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-outline-gradient:hover { background: var(--primary-gradient); color: white; border-color: transparent; transform: translateY(-3px); }
        .form-control, .form-select { border-radius: 10px; border: 2px solid #e9ecef; padding: 0.75rem 1rem; transition: 0.25s; }
        .form-control:focus, .form-select:focus { border-color: #667eea; box-shadow: 0 0 0 0.2rem rgba(102,126,234,0.15); }
        .form-label { font-weight: 600; font-size: 0.9rem; margin-bottom: 0.45rem; }
        .table-modern { border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.06); }
        .table-modern thead { background: var(--primary-gradient); color: white; }
        .table-modern tbody tr:hover { background-color: #f0f4ff; }
        .empty-state { text-align: center; padding: 3rem; color: #6c757d; }
        .empty-state i { font-size: 4rem; color: #dee2e6; margin-bottom: 1rem; }
        .toast-container { position: fixed; top: 20px; right: 20px; z-index: 9999; }
        .toast-message { padding: 1rem 1.5rem; border-radius: 10px; color: white; margin-bottom: 10px; box-shadow: 0 4px 16px rgba(0,0,0,0.15); animation: slideInRight 0.5s ease; display: flex; align-items: center; gap: 10px; }
        .toast-message.success { background: #28a745; }
        .toast-message.error { background: #dc3545; }
        .toast-message.warning { background: #ffc107; color: #333; }
        .toast-message.info { background: #17a2b8; }
        @keyframes slideInRight { from { transform: translateX(100%); opacity: 0; } to { transform: translateX(0); opacity: 1; } }
        @keyframes slideOutRight { from { transform: translateX(0); opacity: 1; } to { transform: translateX(100%); opacity: 0; } }
        .team-member-chip { display: inline-block; background: #f0f4ff; color: var(--primary-color); padding: 0.3rem 0.8rem; border-radius: 20px; margin: 0.2rem; font-size: 0.9rem; }
        .project-card { border-left: 4px solid var(--primary-color); padding: 0.8rem 1rem; margin-bottom: 0.8rem; background: #f8f9fa; border-radius: 8px; transition: all 0.3s; }
        .project-card:hover { background: #f0f4ff; }
        .status-badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
        .status-draft { background: #e9ecef; color: #495057; }
        .status-active { background: #28a745; color: white; }
        .status-submitted { background: #ffc107; color: #856404; }
        .status-graded { background: #17a2b8; color: white; }
        .session-card { border-left: 4px solid var(--primary-color); padding: 0.8rem 1rem; margin-bottom: 0.8rem; background: #f8f9fa; border-radius: 8px; transition: all 0.3s; }
        .session-active { border-left-color: #28a745; }
        .session-upcoming { border-left-color: #ffc107; }
        .session-ended { border-left-color: #dc3545; }
        @media (max-width: 768px) { .stat-card { margin-bottom: 1rem; } }
    </style>

    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-person-square me-2"></i>Student Dashboard</h2>
            <span class="badge bg-primary fs-6"><asp:Label ID="lblClassName" runat="server" Text="Not Enrolled" /></span>
        </div>

        <div class="row g-4 mb-4">
            <div class="col-md-3 col-sm-6">
                <div class="stat-card d-flex align-items-center">
                    <div class="stat-icon me-3"><i class="bi bi-people"></i></div>
                    <div><div class="stat-number"><asp:Label ID="lblTeamCount" runat="server" Text="0" /></div><div class="stat-label">My Teams</div></div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6">
                <div class="stat-card d-flex align-items-center">
                    <div class="stat-icon me-3"><i class="bi bi-folder"></i></div>
                    <div><div class="stat-number"><asp:Label ID="lblProjectCount" runat="server" Text="0" /></div><div class="stat-label">My Projects</div></div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6">
                <div class="stat-card d-flex align-items-center">
                    <div class="stat-icon me-3"><i class="bi bi-star"></i></div>
                    <div><div class="stat-number"><asp:Label ID="lblAvgGrade" runat="server" Text="0%" /></div><div class="stat-label">Average Grade</div></div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6">
                <div class="stat-card d-flex align-items-center">
                    <div class="stat-icon me-3"><i class="bi bi-calendar-check"></i></div>
                    <div><div class="stat-number"><asp:Label ID="lblAttendance" runat="server" Text="0%" /></div><div class="stat-label">Attendance</div></div>
                </div>
            </div>
        </div>

        <div class="card p-3 mb-4">
            <h5><i class="bi bi-people me-2"></i>My Teams</h5>
            <asp:Repeater ID="rptTeams" runat="server">
                <ItemTemplate>
                    <div class="project-card">
                        <div class="d-flex justify-content-between align-items-center flex-wrap">
                            <div><strong><%# Eval("TeamName") %></strong><span class="badge bg-secondary ms-2"><%# Eval("ProjectTitle") %></span></div>
                            <span class="badge bg-info"><%# Eval("MemberCount") %> members</span>
                        </div>
                        <div class="mt-1">
                            <span class="text-muted">Team Members:</span>
                            <span class="team-member-chip"><i class="bi bi-person"></i> <%# Eval("MemberNames") %></span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlEmptyTeams" runat="server" CssClass="empty-state" Visible="false">
                <i class="bi bi-people"></i><p>You are not in any team yet.</p>
            </asp:Panel>
        </div>

        <div class="row">
            <div class="col-md-6">
                <div class="card p-3">
                    <h5><i class="bi bi-folder me-2"></i>My Projects</h5>
                    <asp:Repeater ID="rptProjects" runat="server">
                        <ItemTemplate>
                            <div class="project-card">
                                <div class="d-flex justify-content-between align-items-center">
                                    <strong><%# Eval("Title") %></strong>
                                    <span class="status-badge status-<%# Eval("StatusClass") %>"><%# Eval("Status") %></span>
                                </div>
                                <div class="small text-muted"><i class="bi bi-calendar"></i> <%# Eval("StartDate", "{0:yyyy-MM-dd}") %> - <%# Eval("EndDate", "{0:yyyy-MM-dd}") %></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlEmptyProjects" runat="server" CssClass="empty-state" Visible="false">
                        <i class="bi bi-folder"></i><p>No projects assigned.</p>
                    </asp:Panel>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card p-3">
                    <h5><i class="bi bi-calendar-event me-2"></i>Today's Sessions</h5>
                    <asp:Repeater ID="rptSessions" runat="server">
                        <ItemTemplate>
                            <div class="session-card session-<%# Eval("StatusClass") %>">
                                <div class="d-flex justify-content-between">
                                    <div><strong><%# Eval("ClassName") %></strong> – <%# Eval("SubjectName") %></div>
                                    <span class="badge bg-<%# Eval("BadgeClass") %>"><%# Eval("Status") %></span>
                                </div>
                                <div class="small"><i class="bi bi-clock"></i> <%# Eval("StartTime", "{0:HH:mm}") %> – <%# Eval("EndTime", "{0:HH:mm}") %></div>
                                <div class="mt-1">
                                    <button class="btn btn-sm btn-gradient" onclick='joinSession(<%# Eval("SessionID") %>)' <%# Eval("CanJoin") %>>
                                        <i class="bi bi-door-open"></i> <%# Eval("JoinText") %>
                                    </button>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlEmptySessions" runat="server" CssClass="empty-state" Visible="false">
                        <i class="bi bi-calendar-x"></i><p>No sessions today.</p>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <script>
        function joinSession(sessionId) {
            var currentTime = new Date();
            var startTime = document.getElementById('sessionStart_' + sessionId)?.value;
            var endTime = document.getElementById('sessionEnd_' + sessionId)?.value;
            if (startTime && endTime) {
                var start = new Date('1970-01-01T' + startTime);
                var end = new Date('1970-01-01T' + endTime);
                var now = new Date();
                var nowTime = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes());
                if (nowTime < start) { showToast('Session has not started yet.', 'warning'); return; }
                if (nowTime > end) { showToast('Session has ended.', 'warning'); return; }
            }
            window.location.href = 'Session.aspx?id=' + sessionId;
        }

        function showToast(message, type) {
            var container = document.getElementById('toastContainer');
            if (!container) { container = document.createElement('div'); container.id = 'toastContainer'; container.className = 'toast-container'; document.body.appendChild(container); }
            var toast = document.createElement('div');
            toast.className = 'toast-message ' + type;
            var icon = type === 'success' ? 'check-circle' : type === 'error' ? 'x-circle' : type === 'warning' ? 'exclamation-triangle' : 'info-circle';
            toast.innerHTML = '<i class="bi bi-' + icon + '"></i> ' + message;
            container.appendChild(toast);
            setTimeout(function () { toast.style.animation = 'slideOutRight 0.5s ease'; setTimeout(function () { toast.remove(); }, 500); }, 4000);
        }
    </script>
</asp:Content>