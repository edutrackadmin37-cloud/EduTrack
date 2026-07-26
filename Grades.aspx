<%@ Page Title="My Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="EduTrack.Student.Grades" %>
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
        .grade-card { background: #f8f9fa; border-radius: 12px; padding: 1rem; margin-bottom: 1rem; border-left: 4px solid var(--primary-color); }
        .grade-card .subject { font-weight: 600; font-size: 1.1rem; }
        .grade-card .score { font-size: 1.5rem; font-weight: 700; }
        .grade-card .score.A { color: #28a745; }
        .grade-card .score.B { color: #17a2b8; }
        .grade-card .score.C { color: #ffc107; }
        .grade-card .score.D { color: #fd7e14; }
        .grade-card .score.F { color: #dc3545; }
        .grade-letter { display: inline-block; width: 40px; height: 40px; border-radius: 50%; text-align: center; line-height: 40px; font-weight: 700; color: white; }
        .grade-letter.A { background: #28a745; }
        .grade-letter.B { background: #17a2b8; }
        .grade-letter.C { background: #ffc107; color: #333; }
        .grade-letter.D { background: #fd7e14; }
        .grade-letter.F { background: #dc3545; }
    </style>

    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-star me-2"></i>My Grades</h2>
            <div>
                <span class="badge bg-primary fs-6 me-2">Overall: <asp:Label ID="lblOverall" runat="server" Text="0%" /></span>
                <span class="badge bg-success fs-6"><i class="bi bi-people"></i> <asp:Label ID="lblClassRank" runat="server" Text="N/A" /></span>
            </div>
        </div>

        <div class="card p-3">
            <asp:Repeater ID="rptGrades" runat="server">
                <ItemTemplate>
                    <div class="grade-card">
                        <div class="d-flex justify-content-between align-items-center flex-wrap">
                            <div>
                                <div class="subject"><%# Eval("SubjectName") %></div>
                                <div class="text-muted small"><%# Eval("ClassName") %></div>
                            </div>
                            <div class="text-end">
                                <div class="score grade-<%# Eval("GradeLetter") %>"><%# Eval("GradeValue") %></div>
                                <span class="grade-letter <%# Eval("GradeLetter") %>"><%# Eval("GradeLetter") %></span>
                            </div>
                        </div>
                        <div class="mt-2">
                            <div class="progress" style="height: 8px;">
                                <div class="progress-bar bg-<%# Eval("GradeLetter") == "A" ? "success" : Eval("GradeLetter") == "B" ? "info" : Eval("GradeLetter") == "C" ? "warning" : Eval("GradeLetter") == "D" ? "warning" : "danger" %>" 
                                     style="width: <%# Eval("GradeValue") %>%"></div>
                            </div>
                        </div>
                        <div class="small text-muted mt-1">
                            <i class="bi bi-file-text"></i> <%# Eval("AssignmentsCompleted") %> assignments &middot;
                            <i class="bi bi-check-circle"></i> <%# Eval("TestsTaken") %> tests
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate><div class="empty-state"><i class="bi bi-star"></i><p>No grades available yet.</p></div></EmptyDataTemplate>
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