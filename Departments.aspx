<%@ Page Title="Departments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="EduTrack.Admin.Departments" %>
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
</style> <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-building me-2"></i>Manage Departments</h2>
            <button class="btn btn-gradient" data-bs-toggle="modal" data-bs-target="#deptModal" onclick="clearForm()"><i class="bi bi-plus-circle"></i> Add Department</button>
        </div>
        <div class="card p-3">
            <asp:GridView ID="gvDepartments" runat="server" CssClass="table table-modern" AutoGenerateColumns="False" DataKeyNames="DepartmentID" OnRowCommand="gvDepartments_RowCommand">
                <Columns>
                    <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button class="btn btn-sm btn-outline-gradient" onclick='editDept("<%# Eval("DepartmentID") %>","<%# Eval("DepartmentName") %>","<%# Eval("Description") %>")'><i class="bi bi-pencil"></i></button>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("DepartmentID") %>' CssClass="btn btn-sm btn-secondary" OnClientClick="return confirm('Delete this department?')"><i class="bi bi-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="bi bi-building"></i><p>No departments found.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="deptModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header"><h5 class="modal-title"><span id="modalTitle">Add Department</span></h5><button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button></div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfDeptID" runat="server" />
                    <div class="mb-3"><label class="form-label">Department Name</label><asp:TextBox ID="txtDeptName" runat="server" CssClass="form-control" /></div>
                    <div class="mb-3"><label class="form-label">Description</label><asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" /></div>
                    <div class="mb-3"><label class="form-label">Head of Department</label><asp:DropDownList ID="ddlHead" runat="server" CssClass="form-select" DataTextField="FullName" DataValueField="UserID" /></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-gradient" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function clearForm() { document.getElementById('<%= hfDeptID.ClientID %>').value = ''; document.getElementById('<%= txtDeptName.ClientID %>').value = ''; document.getElementById('<%= txtDescription.ClientID %>').value = ''; document.getElementById('modalTitle').innerText = 'Add Department'; }

        function editDept(id, name, desc) {
            document.getElementById('<%= hfDeptID.ClientID %>').value = id;
            document.getElementById('<%= txtDeptName.ClientID %>').value = name;
            document.getElementById('<%= txtDescription.ClientID %>').value = desc;
            document.getElementById('modalTitle').innerText = 'Edit Department';
            var modal = new bootstrap.Modal(document.getElementById('deptModal'));
            modal.show();
        }
        function showToast(message, type) { /* same as before */ }
    </script>
</asp:Content>