<%@ Page Title="Classes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Classes.aspx.cs" Inherits="EduTrack.Admin.Classes" %>
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
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-book me-2"></i>Manage Classes</h2>
            <button class="btn btn-gradient" data-bs-toggle="modal" data-bs-target="#classModal" onclick="clearForm()"><i class="bi bi-plus-circle"></i> Add Class</button>
        </div>
        <div class="card p-3">
            <asp:GridView ID="gvClasses" runat="server" CssClass="table table-modern" AutoGenerateColumns="False" DataKeyNames="ClassID" OnRowCommand="gvClasses_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                    <asp:BoundField DataField="YearName" HeaderText="Academic Year" />
                    <asp:BoundField DataField="ProgrammeName" HeaderText="Programme" />
                    <asp:BoundField DataField="StreamName" HeaderText="Stream" />
                    <asp:BoundField DataField="ClassTeacherName" HeaderText="Class Teacher" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button class="btn btn-sm btn-outline-gradient" onclick='editClass("<%# Eval("ClassID") %>","<%# Eval("ClassName") %>","<%# Eval("AcademicYearID") %>","<%# Eval("ProgrammeID") %>","<%# Eval("StreamID") %>","<%# Eval("ClassTeacherID") %>")'><i class="bi bi-pencil"></i></button>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ClassID") %>' CssClass="btn btn-sm btn-secondary" OnClientClick="return confirm('Delete this class?')"><i class="bi bi-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="bi bi-book"></i><p>No classes found.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="classModal" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><i class="bi bi-book-plus me-2"></i><span id="modalTitle">Add Class</span></h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfClassID" runat="server" />
                    <div class="mb-3"><label class="form-label">Class Name</label><asp:TextBox ID="txtClassName" runat="server" CssClass="form-control" /></div>
                    <div class="mb-3"><label class="form-label">Academic Year</label><asp:DropDownList ID="ddlAcademicYear" runat="server" CssClass="form-select" DataTextField="YearName" DataValueField="AcademicYearID" /></div>
                    <div class="mb-3"><label class="form-label">Programme</label><asp:DropDownList ID="ddlProgramme" runat="server" CssClass="form-select" DataTextField="ProgrammeName" DataValueField="ProgrammeID" /></div>
                    <div class="mb-3"><label class="form-label">Stream</label><asp:DropDownList ID="ddlStream" runat="server" CssClass="form-select" DataTextField="StreamName" DataValueField="StreamID" /></div>
                    <div class="mb-3"><label class="form-label">Class Teacher</label><asp:DropDownList ID="ddlTeacher" runat="server" CssClass="form-select" DataTextField="FullName" DataValueField="UserID" /></div>
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
            document.getElementById('<%= hfClassID.ClientID %>').value = '';
            document.getElementById('<%= txtClassName.ClientID %>').value = '';
            document.getElementById('modalTitle').innerText = 'Add Class';
        }

        function editClass(id, name, yearId, progId, streamId, teacherId) {
            document.getElementById('<%= hfClassID.ClientID %>').value = id;
            document.getElementById('<%= txtClassName.ClientID %>').value = name;
            document.getElementById('<%= ddlAcademicYear.ClientID %>').value = yearId;
            document.getElementById('<%= ddlProgramme.ClientID %>').value = progId;
            document.getElementById('<%= ddlStream.ClientID %>').value = streamId;
            document.getElementById('<%= ddlTeacher.ClientID %>').value = teacherId || '0';
            document.getElementById('modalTitle').innerText = 'Edit Class';
            var modal = new bootstrap.Modal(document.getElementById('classModal'));
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