<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="EduTrack.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .error-container {
        max-width: 600px;
        margin: 80px auto;
        text-align: center;
        padding: 2rem;
    }
    .error-icon { font-size: 5rem; color: #ffc107; display: block; margin-bottom: 1.5rem; }
    .error-code { font-size: 6rem; font-weight: 700; color: #6c757d; line-height: 1; }
    .error-title { font-size: 2.2rem; font-weight: 700; margin: 0.5rem 0; }
    .error-description { font-size: 1.1rem; color: #6c757d; margin-bottom: 2rem; }
    .btn-gradient { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
    .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
</style>
    <div class="container text-center py-5">
        <i class="bi bi-exclamation-triangle-fill display-1 text-danger"></i>
        <h1 class="mt-3">Oops! Something went wrong.</h1>
        <p class="lead text-muted">We apologize for the inconvenience. Please try again later.</p>
        <p class="text-muted">Error ID: <asp:Label ID="lblErrorId" runat="server" Text="N/A" /></p>
        <a href="~/Default.aspx" class="btn btn-gradient mt-3"><i class="bi bi-house"></i> Go Home</a>
    </div>
</asp:Content>