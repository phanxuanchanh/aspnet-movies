<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Web.Account.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lấy lại mật khẩu</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" type="image/png" href="<% = ResolveUrl("~/account_assets/images/favicon.png") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/account_assets/css/reset-password.css") %>">
</head>
<body>
    <form id="frmResetPassword" runat="server">
        <div class="reset-password-form">
            <div class="reset-password-form-title">
                <h3>Đặt lại mật khẩu</h3>
                <p>Lấy lại mật khẩu để truy cập lại tài khoản</p>
                <hr />
            </div>
            <div class="reset-password-form-data">
                <div class="reset-password-form-group">
                    <p>Địa chỉ Email</p>
                    <asp:TextBox ID="txtEmail" TextMode="Email" Text="" placeholder="Nhập vào địa chỉ Email" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <span>
                            <asp:CustomValidator ID="cvEmail" runat="server"></asp:CustomValidator>
                        </span>
                    </div>
                </div>
            </div>
            <div class="reset-password-form-submit">
                <asp:Button ID="btnSubmit" CssClass="button button-red button-reset-password" runat="server" Text="Xác nhận" />
            </div>
        </div>
    </form>
    <% if (enableShowResult)
        { %>
    <script type="text/javascript">
        var stateDetail = "<% = stateDetail %>";
        setTimeout(function () {
            alert(stateDetail);
        }, 1500);
    </script>
    <%} %>
</body>
</html>
