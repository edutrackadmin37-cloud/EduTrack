<%@ Page Title="Late Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LateReport.aspx.cs" Inherits="EduTrack.Admin.LateReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
    .card { border: none; box-shadow: 0 8px 32px rgba(0,0,0,0.08); border-radius: 12px; background: rgba(255,255,255,0.95); }
    .btn-gradient { background: var(--primary-gradient); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
    .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
    .form-control, .form-select { border-radius: 10px; border: 2px solid #e9ecef; padding: 0.75rem 1rem; }
    .table-modern { border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.06); }
    .table-modern thead { background: var(--primary-gradient); color: white; }
    .toast-container { position: fixed; top: 20px; right: 20px; z-index: 9999; }
    .toast-message { padding: 1rem 1.5rem; border-radius: 10px; color: white; margin-bottom: 10px; box-shadow: 0 4px 16px rgba(0,0,0,0.15); animation: slideInRight 0.5s ease; display: flex; align-items: center; gap: 10px; }
    .toast-message.success { background: #28a745; }
    .toast-message.error { background: #dc3545; }
    @keyframes slideInRight { from { transform: translateX(100%); opacity: 0; } to { transform: translateX(0); opacity: 1; } }
    @keyframes slideOutRight { from { transform: translateX(0); opacity: 1; } to { transform: translateX(100%); opacity: 0; } }
</style>

    <div class="container py-4">
        <h2 class="mb-4"><i class="bi bi-clock-history me-2"></i>Late Arrival Report</h2>
        <div class="card p-3 mb-3">
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">From</label>
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">To</label>
                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Minimum Lates</label>
                    <asp:TextBox ID="txtMinLates" runat="server" CssClass="form-control" Text="1" />
                </div>
                <div class="col-md-3 d-flex align-items-end">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-gradient" OnClick="btnGenerate_Click" />
                    <asp:Button ID="btnExport" runat="server" Text="Export CSV" CssClass="btn btn-outline-gradient ms-2" OnClick="btnExport_Click" />
                </div>
            </div>
        </div>
        <div class="card p-3">
            <asp:GridView ID="gvReport" runat="server" CssClass="table table-modern" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="StudentName" HeaderText="Student" />
                    <asp:BoundField DataField="ClassName" HeaderText="Class" />
                    <asp:BoundField DataField="LateCount" HeaderText="Late Days" />
                    <asp:BoundField DataField="TotalDays" HeaderText="Total Days" />
                    <asp:BoundField DataField="LatePercentage" HeaderText="Late %" DataFormatString="{0:F1}" />
                </Columns>
                <EmptyDataTemplate><div class="empty-state">No data found.</div></EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    <script> 
        function showToast(message, type) {
            var container = document.getElementById('toastContainer');
            if (!container) { container = document.createElement('div'); container.id = 'toastContainer'; container.className = 'toast-container'; document.body.appendChild(container); }
            var toast = document.createElement('div');
            toast.className = 'toast-message ' + type;
            toast.innerHTML = '<i class="bi bi-' + (type === 'success' ? 'check-circle' : 'error' ? 'x-circle' : 'info-circle') + '"></i> ' + message;
            container.appendChild(toast);
            setTimeout(function () { toast.style.animation = 'slideOutRight 0.5s ease'; setTimeout(function () { toast.remove(); }, 500); }, 4000);
        }
    </script>
</asp:Content>