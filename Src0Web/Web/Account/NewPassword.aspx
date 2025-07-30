<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="NewPassword.aspx.cs" Inherits="Web.Account.NewPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lấy lại mật khẩu</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" type="image/png" href="<% = ResolveUrl("~/account_assets/images/favicon.png") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/account_assets/css/reset-password.css") %>">
</head>
<body>
    <form id="frmNewPassword" runat="server">
        <div class="reset-password-form">
            <div class="reset-password-form-title">
                <h3>Đặt lại mật khẩu</h3>
                <p>Lấy lại mật khẩu để truy cập lại tài khoản</p>
                <hr />
            </div>
            <div class="reset-password-form-data">
                <div class="reset-password-form-group">
                    <p>Nhập mật khẩu mới</p>
                    <asp:TextBox ID="txtNewPassword" TextMode="Password" Text="" placeholder="Nhập vào mật khẩu mới" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <span>
                            <asp:CustomValidator ID="cvPassword" runat="server"></asp:CustomValidator>
                        </span>
                    </div>
                </div>
            </div>
            <div class="reset-password-form-submit">
                <asp:Button ID="btnSubmit" CssClass="button button-red button-reset-password" runat="server" Text="Xác nhận" />
            </div>
        </div>
    </form>
</body>
</html>
