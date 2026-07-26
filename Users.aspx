<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="EduTrack.Admin.Users" %>
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
        .modal-content { border-radius: 16px; border: none; }
        .modal-header { background: var(--primary-gradient); color: white; border-radius: 16px 16px 0 0; }
        .btn-close-white { filter: invert(1); }
        .status-badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
        .status-approved { background: #28a745; color: white; }
        .status-pending { background: #ffc107; color: #856404; }
        .status-rejected { background: #dc3545; color: white; }
    </style>

    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-people me-2"></i>User Management</h2>
            <button class="btn btn-gradient" data-bs-toggle="modal" data-bs-target="#userModal" onclick="clearForm()"><i class="bi bi-plus-circle"></i> Add User</button>
        </div>

        <div class="card p-3">
            <asp:GridView ID="gvUsers" runat="server" CssClass="table table-modern" AutoGenerateColumns="False" DataKeyNames="UserID" OnRowCommand="gvUsers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Role" HeaderText="Role" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="status-badge status-<%# Eval("ApprovalStatus").ToString().ToLower() %>"><%# Eval("ApprovalStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button class="btn btn-sm btn-outline-gradient" onclick='editUser("<%# Eval("UserID") %>","<%# Eval("FullName") %>","<%# Eval("Email") %>","<%# Eval("Role") %>","<%# Eval("IsActive") %>")'><i class="bi bi-pencil"></i></button>
                            <asp:LinkButton ID="lnkApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserID") %>' CssClass="btn btn-sm btn-success"><i class="bi bi-check-lg"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserID") %>' CssClass="btn btn-sm btn-danger"><i class="bi bi-x-lg"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("UserID") %>' CssClass="btn btn-sm btn-secondary" OnClientClick="return confirm('Delete this user?')"><i class="bi bi-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="bi bi-people"></i><p>No users found.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="userModal" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><i class="bi bi-person-plus me-2"></i><span id="modalTitle">Add User</span></h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfUserId" runat="server" />
                    <div class="mb-3"><label class="form-label">Full Name</label><asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" /></div>
                    <div class="mb-3"><label class="form-label">Email</label><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" /></div>
                    <div class="mb-3"><label class="form-label">Role</label><asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select"><asp:ListItem>SystemAdministrator</asp:ListItem><asp:ListItem>Teacher</asp:ListItem><asp:ListItem>Student</asp:ListItem><asp:ListItem>Parent</asp:ListItem></asp:DropDownList></div>
                    <div class="mb-3"><label class="form-label">Password (leave blank to keep existing)</label><asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" /></div>
                    <div class="mb-3"><div class="form-check"><asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" /><label class="form-check-label">Active</label></div></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-gradient" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function clearForm() {
            document.getElementById('<%= hfUserId.ClientID %>').value = '';
            document.getElementById('<%= txtFullName.ClientID %>').value = '';
            document.getElementById('<%= txtEmail.ClientID %>').value = '';
            document.getElementById('<%= ddlRole.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= txtPassword.ClientID %>').value = '';
            document.getElementById('<%= chkIsActive.ClientID %>').checked = true;
            document.getElementById('modalTitle').innerText = 'Add User';
        }

        function editUser(id, name, email, role, active) {
            document.getElementById('<%= hfUserId.ClientID %>').value = id;
            document.getElementById('<%= txtFullName.ClientID %>').value = name;
            document.getElementById('<%= txtEmail.ClientID %>').value = email;
            document.getElementById('<%= ddlRole.ClientID %>').value = role;
            document.getElementById('<%= chkIsActive.ClientID %>').checked = active === 'True';
            document.getElementById('modalTitle').innerText = 'Edit User';
            var modal = new bootstrap.Modal(document.getElementById('userModal'));
            modal.show();
        }

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