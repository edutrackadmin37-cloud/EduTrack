<%@ Page Title="Attendance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="EduTrack.Parent.Attendance" %>
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
        .attendance-summary { background: #f8f9fa; border-radius: 12px; padding: 1.5rem; margin-bottom: 1.5rem; }
        .attendance-summary .stat { text-align: center; }
        .attendance-summary .stat .number { font-size: 2rem; font-weight: 700; }
        .attendance-summary .stat .label { color: #6c757d; }
        .status-present { color: #28a745; }
        .status-absent { color: #dc3545; }
        .status-late { color: #ffc107; }
        .status-excused { color: #17a2b8; }
    </style>

    <div class="container py-4">
        <h2 class="mb-4"><i class="bi bi-calendar-check me-2"></i>Attendance</h2>

        <div class="card p-3 mb-4">
            <div class="row">
                <div class="col-md-4">
                    <label class="form-label">Child</label>
                    <asp:DropDownList ID="ddlChild" runat="server" CssClass="form-select" DataTextField="FullName" DataValueField="UserID" AutoPostBack="true" OnSelectedIndexChanged="ddlChild_SelectedIndexChanged" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">From Date</label>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">To Date</label>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-12 mt-3">
                    <asp:Button ID="btnFilter" runat="server" Text="Apply Filters" CssClass="btn btn-gradient" OnClick="btnFilter_Click" />
                </div>
            </div>
        </div>

        <asp:Panel ID="pnlSummary" runat="server" Visible="false">
            <div class="attendance-summary">
                <div class="row">
                    <div class="col-md-3 stat"><div class="number"><asp:Label ID="lblAttendanceRate" runat="server" Text="0%" /></div><div class="label">Attendance Rate</div></div>
                    <div class="col-md-3 stat"><div class="number"><asp:Label ID="lblPresent" runat="server" Text="0" /></div><div class="label">Present</div></div>
                    <div class="col-md-3 stat"><div class="number"><asp:Label ID="lblAbsent" runat="server" Text="0" /></div><div class="label">Absent</div></div>
                    <div class="col-md-3 stat"><div class="number"><asp:Label ID="lblLate" runat="server" Text="0" /></div><div class="label">Late</div></div>
                </div>
            </div>
        </asp:Panel>

        <div class="card p-3">
            <h5><i class="bi bi-clock-history me-2"></i>Attendance Records</h5>
            <asp:GridView ID="gvAttendance" runat="server" CssClass="table table-modern" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="AttendanceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="ClassName" HeaderText="Class" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><span class="status-<%# Eval("Status").ToString().ToLower() %>"><%# Eval("Status") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="bi bi-calendar-x"></i><p>No attendance records found.</p></div></EmptyDataTemplate>
            </asp:GridView>
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