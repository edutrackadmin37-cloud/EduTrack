<%@ Page Title="Access Denied" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error403.aspx.cs" Inherits="EduTrack.Error403" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .error-container {
            max-width: 600px;
            margin: 80px auto;
            text-align: center;
            padding: 2rem;
        }
        .error-icon { font-size: 5rem; color: #dc3545; display: block; margin-bottom: 1.5rem; }
        .error-code { font-size: 6rem; font-weight: 700; color: #6c757d; line-height: 1; }
        .error-title { font-size: 2.2rem; font-weight: 700; margin: 0.5rem 0; }
        .error-description { font-size: 1.1rem; color: #6c757d; margin-bottom: 2rem; }
        .btn-gradient { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
    </style>
    <div class="error-container">
        <span class="error-icon"><i class="bi bi-shield-exclamation"></i></span>
        <div class="error-code">403</div>
        <h1 class="error-title">Access Denied</h1>
        <p class="error-description">You do not have permission to access this page. Please contact your administrator if you believe this is an error.</p>
        <a href="~/Default.aspx" class="btn btn-gradient"><i class="bi bi-house"></i> Go Home</a>
        <a href="javascript:history.back()" class="btn btn-outline-secondary ms-2"><i class="bi bi-arrow-left"></i> Go Back</a>
    </div>
</asp:Content>