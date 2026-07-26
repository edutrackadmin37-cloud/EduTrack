<%@ Page Title="Child Performance" Language="C#" MasterPageFile="~/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ChildPerformance.aspx.cs" Inherits="EduTrack.Parent.ChildPerformance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }
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
        @keyframes slideInRight { from { transform: translateX(100%); opacity: 0; } to { transform: translateX(0); opacity: 1; } }
        @keyframes slideOutRight { from { transform: translateX(0); opacity: 1; } to { transform: translateX(100%); opacity: 0; } }
        .subject-grade { border-left: 4px solid var(--primary-color); padding: 0.8rem 1rem; margin-bottom: 0.8rem; background: #f8f9fa; border-radius: 8px; transition: all 0.3s; }
        .subject-grade:hover { background: #f0f4ff; }
        .subject-grade .subject { font-weight: 600; }
        .subject-grade .score { font-weight: 700; }
        .score-A { color: #28a745; }
        .score-B { color: #17a2b8; }
        .score-C { color: #ffc107; }
        .score-D { color: #fd7e14; }
        .score-F { color: #dc3545; }
        .progress { height: 8px; border-radius: 4px; }
        .grade-letter { display: inline-block; width: 36px; height: 36px; border-radius: 50%; text-align: center; line-height: 36px; font-weight: 700; color: white; }
        .grade-letter.A { background: #28a745; }
        .grade-letter.B { background: #17a2b8; }
        .grade-letter.C { background: #ffc107; color: #333; }
        .grade-letter.D { background: #fd7e14; }
        .grade-letter.F { background: #dc3545; }
        .child-info { background: var(--primary-gradient); color: white; padding: 1rem; border-radius: 12px; margin-bottom: 1.5rem; }
        .child-info .name { font-size: 1.5rem; font-weight: 700; }
        .child-info .detail { opacity: 0.9; }
    </style>

    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-person me-2"></i>Child Performance</h2>
            <a href="Dashboard.aspx" class="btn btn-outline-gradient"><i class="bi bi-arrow-left"></i> Back</a>
        </div>

        <div class="child-info">
            <div class="row">
                <div class="col-md-4"><div class="name"><asp:Label ID="lblChildName" runat="server" Text="-" /></div><div class="detail"><asp:Label ID="lblClass" runat="server" Text="-" /></div></div>
                <div class="col-md-4"><div class="detail">Attendance</div><div><strong><asp:Label ID="lblAttendance" runat="server" Text="0%" /></strong></div></div>
                <div class="col-md-4"><div class="detail">Overall Average</div><div><strong><asp:Label ID="lblOverallAvg" runat="server" Text="0%" /></strong></div></div>
            </div>
        </div>

        <div class="card p-3">
            <h5><i class="bi bi-book me-2"></i>Subject Performance (Siloed)</h5>
            <p class="text-muted small">Each subject's performance is displayed separately. Scores are never combined across subjects.</p>
            <asp:Repeater ID="rptSubjects" runat="server">
                <ItemTemplate>
                    <div class="subject-grade">
                        <div class="d-flex justify-content-between align-items-center flex-wrap">
                            <div>
                                <span class="subject"><%# Eval("SubjectName") %></span>
                                <span class="text-muted small ms-2"><%# Eval("SubjectCode") %></span>
                            </div>
                            <div>
                                <span class="score score-<%# GetGradeLetter(Eval("AverageGrade")) %>">
                                    <%# Eval("AverageGrade", "{0:F1}") %>% 
                                </span>
                                <span class="grade-letter <%# GetGradeLetter(Eval("AverageGrade")) %>">
                                    <%# GetGradeLetter(Eval("AverageGrade")) %>
                                </span>
                            </div>
                        </div>
                        <div class="mt-1">
                            <div class="progress">
                                <div class="progress-bar bg-<%# GetProgressClass(Eval("AverageGrade")) %>" 
                                     style="width: <%# Eval("AverageGrade") %>%"></div>
                            </div>
                        </div>
                        <div class="mt-1 small text-muted">
                            <i class="bi bi-file-text"></i> <%# Eval("AssignmentsCompleted") %> assignments &middot;
                            <i class="bi bi-check-circle"></i> <%# Eval("TestsTaken") %> tests &middot;
                            <i class="bi bi-calendar-check"></i> Attendance: <%# Eval("AttendanceRate", "{0:F0}%") %>
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="empty-state"><i class="bi bi-book"></i><p>No subject performance data available for this child.</p></div>
                </EmptyDataTemplate>
            </asp:Repeater>
        </div>
    </div>

    <script>
        function showToast(message, type) {
            var container = document.getElementById('toastContainer');
            if (!container) { container = document.createElement('div'); container.id = 'toastContainer'; container.className = 'toast-container'; document.body.appendChild(container); }
            var toast = document.createElement('div');
            toast.className = 'toast-message ' + type;
            toast.innerHTML = '<i class="bi bi-' + (type === 'success' ? 'check-circle' : type === 'error' ? 'x-circle' : 'exclamation-triangle') + '"></i> ' + message;
            container.appendChild(toast);
            setTimeout(function () { toast.style.animation = 'slideOutRight 0.5s ease'; setTimeout(function () { toast.remove(); }, 500); }, 4000);
        }
    </script>
</asp:Content>