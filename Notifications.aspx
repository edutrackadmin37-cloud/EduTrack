<%@ Page Title="Notifications" Language="C#" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" MasterPageFile="~/Site.Master" Inherits="EduTrack.Notifications" %>
<asp:Content ID="NotificationsContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
body {background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);}
.notif-container {max-width: 920px; margin: 52px auto;}
.auth-card {border: none; border-radius: 20px; background: #fff; box-shadow: 0 10px 32px rgba(0,0,0,0.15);}
.notif-header {background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);color:white;padding:1.2rem 2rem;}
.notif-header h2 {margin: 0; font-weight:700;}
.notif-body {padding:2.1rem 1.4rem;}
.btn-auth {background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);font-weight:700;border:none;border-radius:12px;}
.btn-auth:hover{background:linear-gradient(135deg,#764ba2 0%,#667eea 100%);}
.table th,.table td{vertical-align:middle;}
</style>

<div class="notif-container auth-card">
    <div class="notif-header">
        <h2>Notifications</h2>
        <span class="fs-6">Announcements &amp; alerts sent to you</span>
    </div>
    <div class="notif-body">
        <asp:Label ID="lblNotifMsg" runat="server" CssClass="alert alert-info" Visible="false"/>
        <asp:Panel ID="pnlSendNotif" runat="server" Visible="false">
            <div class="row mb-3">
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlNotifTarget" runat="server" CssClass="form-control">
                        <asp:ListItem Value="All">All Users</asp:ListItem>
                        <asp:ListItem Value="Teachers">Teachers</asp:ListItem>
                        <asp:ListItem Value="Students">Students</asp:ListItem>
                        <asp:ListItem Value="Parents">Parents</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-8">
                    <asp:TextBox ID="txtNotText" runat="server" CssClass="form-control" placeholder="Notification text" MaxLength="255"/>
                </div>
                <div class="col-md-2 text-end">
                    <asp:Button ID="btnSend" runat="server" Text="Send Notification" CssClass="btn btn-auth w-100" OnClick="btnSend_Click"/>
                </div>
            </div>
        </asp:Panel>
        <h5 class="mb-2">Your Notifications</h5>
        <asp:GridView ID="gvNotifs" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
            DataKeyNames="NotificationID">
            <Columns>
                <asp:BoundField DataField="NotificationText" HeaderText="Message"/>
                <asp:BoundField DataField="NotificationDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}"/>
                <asp:BoundField DataField="ReadStatus" HeaderText="Status"/>
            </Columns>
        </asp:GridView>
        <div class="d-flex justify-content-between mt-4">
            <a href="Messages.aspx" class="btn btn-outline-secondary"><i class="bi bi-arrow-left"></i> Back</a>
        </div>
    </div>
</div>
</asp:Content>