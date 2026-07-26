<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="EduTrack.Parent.Notifications" %>
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
        .notification-item { border-left: 4px solid var(--primary-color); padding: 0.8rem 1rem; margin-bottom: 0.8rem; background: #f8f9fa; border-radius: 8px; transition: all 0.3s; }
        .notification-item:hover { background: #f0f4ff; }
        .notification-item .text { font-weight: 500; }
        .notification-item .date { font-size: 0.8rem; color: #6c757d; }
        .notification-item.unread { border-left-color: #ffc107; background: #fff8e1; }
        .notification-item.unread .text { font-weight: 600; }
    </style>

    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <h2><i class="bi bi-bell me-2"></i>Notifications</h2>
            <div>
                <span class="badge bg-primary fs-6 me-2"><asp:Label ID="lblUnread" runat="server" Text="0" /> unread</span>
                <asp:Button ID="btnMarkAllRead" runat="server" Text="Mark All as Read" CssClass="btn btn-gradient" OnClick="btnMarkAllRead_Click" />
            </div>
        </div>

        <div class="card p-3">
            <asp:Repeater ID="rptNotifications" runat="server">
                <ItemTemplate>
                    <div class="notification-item <%# GetUnreadClass(Eval("IsRead")) %>">
                        <div class="d-flex justify-content-between">
                            <span class="text"><%# Eval("NotificationText") %></span>
                            <span class="date"><%# Eval("NotificationDate", "{0:yyyy-MM-dd HH:mm}") %></span>
                        </div>
                        <div class="mt-1">
                            <asp:LinkButton ID="lnkMarkRead" runat="server" CommandName="MarkRead" 
                                CommandArgument='<%# Eval("NotificationID") %>' 
                                CssClass="btn btn-sm btn-outline-gradient me-2" 
                                Visible='<%# IsUnread(Eval("IsRead")) %>'>
                                <i class="bi bi-check2"></i> Mark Read
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("NotificationID") %>' 
                                CssClass="btn btn-sm btn-secondary" 
                                OnClientClick="return confirm('Delete this notification?')">
                                <i class="bi bi-trash"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="empty-state"><i class="bi bi-bell"></i><p>No notifications.</p></div>
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