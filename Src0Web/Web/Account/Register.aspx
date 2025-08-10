<%@ Page Async="true" Language="C#" AutoEventWireup="true" MasterPageFile="~/Account/Layouts/AccountLayout.Master" CodeBehind="Register.aspx.cs" Inherits="Web.Account.Register" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Account/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Đăng ký</title>
    <style>
        body {
            background-color: #f8f9fa;
        }

        .login-container {
            max-width: 800px;
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
        <uc:NotifControl ID="notifControl" runat="server" />
        <h3 class="text-center mb-4">Đăng ký tài khoản</h3>
        <div class="mb-3">
            <label for="txtUsername" class="form-label">Tên đăng nhập</label>
            <asp:TextBox ID="txtUsername" CssClass="form-control" Text="" Placeholder="Nhập vào tên người dùng" TextMode="SingleLine" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="cvUsername" CssClass="text-danger" runat="server"></asp:CustomValidator>
        </div>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="txtEmail" class="form-label">Địa chỉ Email</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" Text="" Placeholder="Nhập vào địa chỉ Email" TextMode="Email" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvEmail" CssClass="text-danger" runat="server"></asp:CustomValidator>
            </div>
            <div class="col-md-6 mb-3">
                <label for="txtPhoneNumber" class="form-label">Số điện thoại</label>
                <asp:TextBox ID="txtPhoneNumber" CssClass="form-control" Text="" Placeholder="Nhập vào số điện thoại" TextMode="SingleLine" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvPhoneNumber" CssClass="text-danger" runat="server"></asp:CustomValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="txtPassword" class="form-label">Mật khẩu</label>
                <asp:TextBox ID="txtPassword" CssClass="form-control" Text="" Placeholder="Nhập vào mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvPassword" CssClass="text-danger" runat="server"></asp:CustomValidator>
            </div>
            <div class="col-md-6 mb-3">
                <label for="txtRePassword" class="form-label">Xác nhận mật khẩu</label>
                <asp:TextBox ID="txtRePassword" CssClass="form-control" Text="" Placeholder="Nhập vào mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="cmpRePassword" runat="server"></asp:CompareValidator>
            </div>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary" Text="Đăng nhập" />
        </div>
        <div class="mt-3">
            <asp:HyperLink ID="hylnkResetPassword" runat="server" CssClass="text-decoration-none">Bạn quên mật khẩu?</asp:HyperLink>
            |<asp:HyperLink ID="hylnkLogin" CssClass="text-decoration-none" runat="server">Đăng nhập?</asp:HyperLink>
        </div>
    </div>
</asp:Content>
