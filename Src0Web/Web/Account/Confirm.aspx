<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Web.Account.Confirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Xác minh tài khoản</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" type="image/png" href="<% = ResolveUrl("~/account_assets/images/favicon.png") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/account_assets/css/confirm.css") %>">
</head>
<body>
    <form id="frmConfirm" runat="server">
        <div class="confirm-form">
            <div class="confirm-form-title">
                <h3>Xác minh tài khoản</h3>
                <p>Bước cuối cùng để truy cập tài khoản</p>
                <hr />
            </div>
            <div class="confirm-form-data">
                <div class="confirm-form-group">
                    <p>Mã xác nhận</p>
                    <asp:TextBox ID="txtConfirmCode" Text="" Placeholder="Nhập vào mã xác nhận" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvConfirmCode" runat="server"></asp:CustomValidator>
                    </div>
                </div>
            </div>
            <div class="confirm-form-submit">
                <asp:Button ID="btnConfirm" CssClass="button button-red button-confirm" runat="server" Text="Xác nhận" />
            </div>
            <div class="confirm-form-support">
                <p>
                    <asp:HyperLink ID="hylnkReConfirm" runat="server">Gửi lại mã xác nhận</asp:HyperLink>
                </p>
            </div>
        </div>
    </form>
    <script src="<%= ResolveUrl("~/account_assets/js/confirm.js") %>"></script>
</body>
</html>
