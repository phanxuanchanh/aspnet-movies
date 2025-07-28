<%@ Page Async="true" Language="C#" AutoEventWireup="true" MasterPageFile="~/Account/Layouts/AccountLayout.Master" CodeBehind="Login.aspx.cs" Inherits="Web.Account.Login" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Đăng nhập</title>
    <style>
        body {
            background-color: #f8f9fa;
        }

        .login-container {
            max-width: 400px;
            margin: 100px auto;
            padding: 30px;
            border-radius: 12px;
            background-color: #ffffff;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="login-container">
        <h3 class="text-center mb-4">Đăng nhập</h3>
        <div class="mb-3">
            <label for="mainContent_txtUsername" class="form-label">Tên đăng nhập</label>
            <asp:TextBox ID="txtUsername" CssClass="form-control" Text="" Placeholder="Nhập vào tên người dùng" TextMode="SingleLine" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="cvUsername" CssClass="text-danger" runat="server"></asp:CustomValidator>
        </div>
        <div class="mb-3">
            <label for="mainContent_password" class="form-label">Mật khẩu</label>
            <asp:TextBox ID="txtPassword" CssClass="form-control" Text="" Placeholder="Nhập vào mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="cvPassword" CssClass="text-danger" runat="server"></asp:CustomValidator>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary" Text="Đăng nhập" />
        </div>
        <div class="mt-3">
            <asp:HyperLink ID="hylnkResetPassword" runat="server" CssClass="text-decoration-none">Bạn quên mật khẩu?</asp:HyperLink>
            |<asp:HyperLink ID="hylnkRegister" CssClass="text-decoration-none" runat="server">Đăng ký mới?</asp:HyperLink>
        </div>
    </div>
</asp:Content>
