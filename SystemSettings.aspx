<%@ Page Title="System Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="EduTrack.Admin.SystemSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <style>
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
    body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; }
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
    .toast-container { position: fixed; top: 20px; right: 20px; z-index: 9999; }
    .toast-message { padding: 1rem 1.5rem; border-radius: 10px; color: white; margin-bottom: 10px; box-shadow: 0 4px 16px rgba(0,0,0,0.15); animation: slideInRight 0.5s ease; display: flex; align-items: center; gap: 10px; }
    .toast-message.success { background: #28a745; }
    .toast-message.error { background: #dc3545; }
    @keyframes slideInRight { from { transform: translateX(100%); opacity: 0; } to { transform: translateX(0); opacity: 1; } }
    @keyframes slideOutRight { from { transform: translateX(0); opacity: 1; } to { transform: translateX(100%); opacity: 0; } }
    .modal-content { border-radius: 16px; border: none; }
    .modal-header { background: var(--primary-gradient); color: white; border-radius: 16px 16px 0 0; }
    .btn-close-white { filter: invert(1); }
</style> 
    <div class="container py-4">
        <h2 class="mb-4"><i class="bi bi-gear me-2"></i>System Settings</h2>

        <div class="card p-4">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block" Visible="false" />

            <div class="row">
                <div class="col-md-6">
                    <div class="mb-3"><label class="form-label">Site Name</label><asp:TextBox ID="txtSiteName" runat="server" CssClass="form-control" /></div>
                    <div class="mb-3"><label class="form-label">Institution Name</label><asp:TextBox ID="txtInstitutionName" runat="server" CssClass="form-control" /></div>
                    <div class="mb-3"><label class="form-label">Contact Email</label><asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" TextMode="Email" /></div>
                    <div class="mb-3"><label class="form-label">Platform URL</label><asp:TextBox ID="txtPlatformURL" runat="server" CssClass="form-control" /></div>
                </div>
                <div class="col-md-6">
                    <div class="mb-3"><label class="form-label">School Year</label><asp:TextBox ID="txtSchoolYear" runat="server" CssClass="form-control" /></div>
                    <div class="mb-3"><label class="form-label">Grading Scale</label><asp:DropDownList ID="ddlGradingScale" runat="server" CssClass="form-select"><asp:ListItem Value="percent">Percentage</asp:ListItem><asp:ListItem Value="letter">Letter</asp:ListItem><asp:ListItem Value="gpa">GPA</asp:ListItem></asp:DropDownList></div>
                    <div class="mb-3"><div class="form-check"><asp:CheckBox ID="chkManualApproval" runat="server" CssClass="form-check-input" /><label class="form-check-label">Manual Account Approval</label></div></div>
                    <div class="mb-3"><div class="form-check"><asp:CheckBox ID="chkEnableChat" runat="server" CssClass="form-check-input" /><label class="form-check-label">Enable Team Chat</label></div></div>
                    <div class="mb-3"><div class="form-check"><asp:CheckBox ID="chkPeerAssessment" runat="server" CssClass="form-check-input" /><label class="form-check-label">Enable Peer Assessment</label></div></div>
                </div>
            </div>

            <div class="mt-3">
                <asp:Button ID="btnSave" runat="server" Text="Save Settings" CssClass="btn btn-gradient" OnClick="btnSave_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset to Default" CssClass="btn btn-outline-gradient ms-2" OnClick="btnReset_Click" CausesValidation="false" />
            </div>
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